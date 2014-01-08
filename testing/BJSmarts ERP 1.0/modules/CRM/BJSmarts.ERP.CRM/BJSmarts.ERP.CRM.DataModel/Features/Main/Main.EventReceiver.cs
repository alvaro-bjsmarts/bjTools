using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.Administration;
using Microsoft.SharePoint.BusinessData.Parser;
using Microsoft.SharePoint.BusinessData.SharedService;
using System.Security.Permissions;

namespace BJSmarts.ERP.CRM.DataModel.Features.Main
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    
    public class MainEventReceiver : SPFeatureReceiver
    {
        private class LobSystemAssemblies
        {
            internal byte[] MainAssembly;
            internal List<byte[]> DependentAssemblies;
        }

        /// <summary>
        /// Suffix used for exported model files.
        /// </summary>
        private const string FileSuffix = "_yyyy_MM_dd_HH_mm_ss_fff";

        /// <summary>
        /// The admin catalog used to import the model
        /// </summary>
        private AdministrationMetadataCatalog amc;

        /// <summary>
        /// Contains the in-memory representation of the model xml file.
        /// </summary>
        private XmlDocument modelDocument = new XmlDocument();

        /// <summary>
        /// Name of the model file.
        /// </summary>
        private string modelFileName;

        /// <summary>
        /// Farm where the feature is being activated.
        /// </summary>
        private SPFarm parentFarm;

        /// <summary>
        /// Feature name.
        /// </summary>
        private string featureName;

        /// <summary>
        /// Activates the feature. It imports the model file, resource model file and the assemblies.
        /// </summary>
        /// <param name="properties">Feature properties.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            if (properties.Definition == null || properties.Definition.Properties == null)
            {
                throw new ArgumentException(
                    "Feature definition is null or feature properties are null.");
            }

            if (properties.Definition.Scope != SPFeatureScope.Farm)
            {
                throw new ArgumentException(
                    "BDC import model feature receiver expects feature be 'Farm' scoped.");
            }

            var siteUrl = "http://localhost:51816";

            // Set site url property so that the external content type can be deployed.
            if (properties.Definition.Properties["SiteUrl"] == null)
            {
                properties.Definition.Properties.Add(new SPFeatureProperty("SiteUrl", siteUrl));
            }

            this.featureName = properties.Definition.DisplayName;

            this.parentFarm = properties.Definition.Farm;

            string featureFolder = GetFeatureFolder(properties.Definition);

            string modelFilePath = GetModelFilePath(properties.Definition.Properties["ModelFileName"], featureFolder);

            string[] resourceFilePaths = GetResourceModelFilesPaths(properties.Definition.Properties["ResourceFiles"], featureFolder);

            this.modelDocument.Load(modelFilePath);

            Dictionary<string, LobSystemAssemblies> lobSystemAssemblies =
                GetLobSystemAssemblies(properties.Definition.Properties, featureFolder);

            bool incremental = GetIncrementalUpdate(properties.Definition.Properties["IncrementalUpdate"]);

            CreateAdministrationMetadataCatalog(properties.Definition.Properties["SiteUrl"]);

            ImportBdcArtifacts(featureFolder, modelFilePath, resourceFilePaths, lobSystemAssemblies, incremental);
        }

        /// <summary>
        /// Deactivate the feature. It deletes the model from the Business Data Catalog.
        /// </summary>
        /// <param name="properties">Feature properties.</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            if (properties.Definition == null || properties.Definition.Properties == null)
            {
                throw new ArgumentException(
                    "Feature definition is null or feature properties are null.");
            }

            var siteUrl = "http://localhost:51816";

            // Set site url property so that the external content type can be deployed.
            if (properties.Definition.Properties["SiteUrl"] == null)
            {
                properties.Definition.Properties.Add(new SPFeatureProperty("SiteUrl", siteUrl));
            }

            this.featureName = properties.Definition.DisplayName;

            this.parentFarm = properties.Definition.Farm;

            string featureFolder = GetFeatureFolder(properties.Definition);

            string modelFilePath = GetModelFilePath(properties.Definition.Properties["ModelFileName"], featureFolder);

            this.modelDocument.Load(modelFilePath);

            bool incremental = GetIncrementalUpdate(properties.Definition.Properties["IncrementalUpdate"]);

            CreateAdministrationMetadataCatalog(properties.Definition.Properties["SiteUrl"]);

            ExportAndDeleteModel(GetModelName(), featureFolder, incremental);
        }

        /// <summary>
        /// Get the value of the IncrementalUpdate flag. If the IncrementalUpdate is not present then it will return false.
        /// </summary>
        /// <param name="incrementalUpdateProperty">SPFeatureProperty for the IncrementalUpdate property.</param>
        /// <returns>Value for the IncrementalUpdate property.</returns>
        private static bool GetIncrementalUpdate(SPFeatureProperty incrementalUpdateProperty)
        {
            // Optional, default=false
            if (null == incrementalUpdateProperty || String.IsNullOrEmpty(incrementalUpdateProperty.Value))
            {
                return false;
            }

            return Convert.ToBoolean(incrementalUpdateProperty.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Saves information about the existing model.
        /// </summary>
        /// <param name="modelName">Name of the model whose information is to be saved.</param>    
        /// <param name="featureFolder">Folder to export model to. Should be the feature folder.</param>    
        /// <param name="exportedLobSystems">Assembly information about the lob systems.</param>
        /// <returns>The exported model contents. Null if model not found.</returns>
        private string SaveExistingModelAndDelete(string modelName, string featureFolder,
            out Dictionary<string, LobSystemAssemblies> exportedLobSystems)
        {
            string exportedModelContents = null;
            exportedLobSystems = null;

            if (string.IsNullOrEmpty(modelName))
            {
                return null;
            }
            Model model = GetModel(modelName);
            if (model == null)
            {
                return null;
            }

            exportedModelContents = this.amc.ExportPackage(modelName, PackageContents.Model |
                PackageContents.Permissions | PackageContents.LocalizedNames | PackageContents.Properties);

            exportedLobSystems = ExportLobSystem(model);

            ExportModel(exportedModelContents, featureFolder);

            DeleteModel(model, false);

            return exportedModelContents;
        }

        /// <summary>
        /// Export the lobSystems of the model.
        /// </summary>
        /// <param name="model">the model</param>
        /// <returns>lob systems</returns>
        private static Dictionary<string, LobSystemAssemblies> ExportLobSystem(Model model)
        {
            LobSystemCollection collection = model.OwnedReferencedLobSystems;

            Dictionary<string, LobSystemAssemblies> exportedLobSystems = new Dictionary<string, LobSystemAssemblies>();
            foreach (LobSystem lobSystem in collection)
            {
                if (lobSystem.HasProxyAssembly)
                {
                    LobSystemAssemblies assemblies = new LobSystemAssemblies();
                    IList<byte[]> allAssemblies = lobSystem.GetAllAssemblies();
                    if (allAssemblies != null && allAssemblies.Count > 0)
                    {
                        assemblies.MainAssembly = allAssemblies[0];
                        if (allAssemblies.Count > 1)
                        {
                            allAssemblies.RemoveAt(0);
                            assemblies.DependentAssemblies = new List<byte[]>(allAssemblies.Count);
                            assemblies.DependentAssemblies.AddRange(allAssemblies);
                        }
                    }
                    exportedLobSystems.Add(lobSystem.Name, assemblies);
                }
            }
            return exportedLobSystems;
        }


        /// <summary>
        /// Imports the saved model.
        /// </summary>
        /// <param name="modelContents">Model contents to be imported.</param>
        /// <param name="lobSystemsAssemblies">Assemblies to be imported.</param>
        /// <param name="incrementalUpdate">Indicates whether to ImportPackage should update the existing model.</param>     
        private void ImportTheSavedModel(string modelContents, Dictionary<string, LobSystemAssemblies> lobSystemsAssemblies, bool incrementalUpdate)
        {
            Model model = ImportModel(modelContents, PackageContents.Model |
                PackageContents.Permissions | PackageContents.LocalizedNames | PackageContents.Properties, incrementalUpdate);

            LobSystemCollection collection = model.AllReferencedLobSystems;
            LobSystemAssemblies assemblies;
            foreach (LobSystem lobSystem in collection)
            {
                if (lobSystemsAssemblies.TryGetValue(lobSystem.Name, out assemblies))
                {
                    if (assemblies.MainAssembly != null)
                    {
                        lobSystem.PersistAssembly(assemblies.MainAssembly, assemblies.DependentAssemblies);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Name of the Model from the specified model file.
        /// </summary>
        /// <returns>Model Name</returns>
        private string GetModelName()
        {
            XmlNodeList modelList = this.modelDocument.GetElementsByTagName("Model");
            if (modelList == null || modelList.Count != 1)
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                    "Model file {0} is not valid. 'Model' root element not found.", this.modelFileName));
            }

            XmlAttribute nameAttribute = modelList[0].Attributes["Name"];
            if (nameAttribute == null)
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                    "Model file {0} is not valid. 'Model' root element does not have a 'Name' attribute.", this.modelFileName));
            }
            return nameAttribute.Value;
        }

        /// <summary>
        /// Returns the full path of the Model file.
        /// </summary>
        /// <param name="featureProperty">ModelFileName feature property.</param>
        /// <param name="featureFolder">Full path of the feature folder.</param>
        /// <returns>Full path of the Model File.</returns>
        private string GetModelFilePath(SPFeatureProperty featureProperty, string featureFolder)
        {
            this.modelFileName = featureProperty.Value;

            string modelFilePath = String.Empty;
            try
            {
                modelFilePath = Path.Combine(featureFolder, this.modelFileName);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "Feature property 'ModelFileName' has invalid value. Feature activation failed with the following message: {0}", ex.Message));
            }

            if (File.Exists(modelFilePath) == false)
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                    "Model file '{0}' does not exist.", modelFilePath));
            }
            return modelFilePath;
        }

        /// <summary>
        /// Returns the full path of the Feature folder.
        /// </summary>
        /// <param name="featureDefinition">Feature definition object.</param>
        /// <returns>Full path of the feature folder.</returns>
        private static string GetFeatureFolder(SPFeatureDefinition featureDefinition)
        {
            if (featureDefinition == null)
            {
                throw new ArgumentNullException("featureDefinition");
            }

            string featureFolder = Path.Combine(SPUtility.GetGenericSetupPath(@"Template\Features"), featureDefinition.DisplayName);
            if (Directory.Exists(featureFolder) == false)
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                    "Feature folder '{0}' does not exists.", featureFolder));
            }
            return featureFolder;
        }

        /// <summary>
        /// Exports model and deletes model if the Model exists.
        /// </summary>
        /// <param name="modelName">Name of the model to be exported and deleted.</param>
        /// <param name="targetDirectory">Target directory to save the model.</param>
        /// <param name="incrementalUpdate">Indicates whether to Delete the model or DeleteNoCascade.</param>
        private void ExportAndDeleteModel(string modelName, string targetDirectory, bool incrementalUpdate)
        {
            Model model = this.GetModel(modelName);
            if (model == null)
            {
                return;
            }

            if (false == incrementalUpdate)
            {
                string modelContents = this.amc.ExportPackage(modelName, PackageContents.Model |
                    PackageContents.Permissions | PackageContents.LocalizedNames | PackageContents.Properties);

                ExportModel(modelContents, targetDirectory);
            }
            DeleteModel(model, incrementalUpdate);
        }

        /// <summary>
        /// Exports the model to a file.
        /// File will be located at the directory specified by targetDirectory and the name of the file 
        /// will be the modelName followed by a current date time stamp.
        /// </summary>
        /// <param name="modelContents">Model contents.</param>
        /// <param name="targetDirectory">Directory to save to exported model.</param>
        private void ExportModel(string modelContents, string targetDirectory)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.modelFileName);
            string exportedModelFilePath = string.Format(CultureInfo.InvariantCulture,
                "{0}{1}.xml",
                Path.Combine(targetDirectory, fileNameWithoutExtension),
                DateTime.Now.ToString(FileSuffix, CultureInfo.InvariantCulture));
            using (StreamWriter sw = new StreamWriter(exportedModelFilePath))
            {
                sw.Write(modelContents);
                sw.Flush();
            }
        }

        /// <summary>
        /// Returns the assembly information for the LobSystems.
        /// </summary>
        /// <param name="featurePropertyCollection">Collection of LobSystem Properties.</param>
        /// <param name="featureFolder">Feature folder.</param>
        /// <returns>Collection of assembly information for the LobSystems.</returns>
        private Dictionary<string, LobSystemAssemblies> GetLobSystemAssemblies(SPFeaturePropertyCollection featurePropertyCollection,
            string featureFolder)
        {
            //Get a list of all lobSystems defined by the model.
            XmlNodeList lobSystemList = this.modelDocument.GetElementsByTagName("LobSystem");

            Dictionary<string, string> lobSystemProperties = new Dictionary<string, string>(lobSystemList.Count);

            foreach (XmlNode node in lobSystemList)
            {
                XmlAttribute lobSystemNameAttribute = node.Attributes["Name"];
                //If lobSystem does not have a Name, import will fail with XSD validation.
                if (lobSystemNameAttribute != null)
                {
                    string lobSystemName = lobSystemNameAttribute.Value;
                    if (featurePropertyCollection[lobSystemName] != null &&
                        !String.IsNullOrEmpty(featurePropertyCollection[lobSystemName].Value))
                    {
                        string lobSystemProperty = featurePropertyCollection[lobSystemName].Value;
                        lobSystemProperties.Add(lobSystemName, lobSystemProperty);
                    }
                }
            }

            Dictionary<string, LobSystemAssemblies> lobSystemAssemblies = new Dictionary<string, LobSystemAssemblies>();

            foreach (string lobSystemName in lobSystemProperties.Keys)
            {
                string[] assembliesPath = ParseSemicolonDelimitedNames(lobSystemProperties[lobSystemName]);

                LobSystemAssemblies assemblies = new LobSystemAssemblies();

                //Add full path to assembly names and load them.
                for (int i = 0; i < assembliesPath.Length; i++)
                {
                    try
                    {
                        assembliesPath[i] = Path.Combine(featureFolder, assembliesPath[i]);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                            "Feature property '{0}' has invalid value. Feature activation failed with the following message: {1}", lobSystemName, ex.Message));
                    }

                    if (File.Exists(assembliesPath[i]) == false)
                    {
                        throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                            "Assembly file '{0}' does not exist.", assembliesPath[i]));
                    }

                    // First assembly is the main assembly
                    if (i == 0)
                    {
                        assemblies.MainAssembly = GetAssemblyBytes(assembliesPath[0]);
                    }
                    else
                    {
                        if (assemblies.DependentAssemblies == null)
                        {
                            assemblies.DependentAssemblies = new List<byte[]>(assembliesPath.Length - 1);
                        }
                        assemblies.DependentAssemblies.Add(GetAssemblyBytes(assembliesPath[i]));
                    }
                }

                lobSystemAssemblies.Add(lobSystemName, assemblies);

            }

            return lobSystemAssemblies;
        }

        /// <summary>
        /// Returns the full paths of all the resource files.
        /// </summary>
        /// <param name="featureProperty">ResourceFileNames property.</param>
        /// <param name="featureFolder">Full path of the feature folder.</param>
        /// <returns>Full paths of the resoucre model files.</returns>
        private static string[] GetResourceModelFilesPaths(SPFeatureProperty featureProperty, string featureFolder)
        {
            //Resources are optional.
            if (featureProperty == null || string.IsNullOrEmpty(featureProperty.Value))
            {
                return null;
            }

            string[] resourceFilesPaths = ParseSemicolonDelimitedNames(featureProperty.Value);

            //Make it into full file name path.
            for (int i = 0; i < resourceFilesPaths.Length; i++)
            {
                try
                {
                    resourceFilesPaths[i] = Path.Combine(featureFolder, resourceFilesPaths[i]);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        "Feature property 'ResourceFiles' has invalid value. Feature activation failed with the following message: {0}", ex.Message));
                }

                if (File.Exists(resourceFilesPaths[i]) == false)
                {
                    throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture,
                        "Resource file {0} does not exist.", resourceFilesPaths[i]));
                }
            }
            return resourceFilesPaths;
        }

        /// <summary>
        /// Imports the Bdc artifacts into the Bdc Store.
        /// </summary>
        /// <param name="featureFolder">Fetaure folder that contains all the files.</param>
        /// <param name="modelFilePath">Full path for the model file.</param>
        /// <param name="resourceFilesPaths">Model resource files.</param>
        /// <param name="lobSystemAssemblies">Information about the assemblies associated with the lob systems.</param>
        /// <param name="incrementalUpdate">Indicates whether to ImportPackage should update the existing model.</param>
        private void ImportBdcArtifacts(string featureFolder, string modelFilePath, string[] resourceFilesPaths,
            Dictionary<string, LobSystemAssemblies> lobSystemAssemblies, bool incrementalUpdate)
        {
            Model model = null;
            bool successful = false;
            string exportedModelContents = null;
            Dictionary<string, LobSystemAssemblies> exportedLobSystems = null;

            try
            {
                string modelName = this.GetModelName();
                bool upgrade = true;

                // For ship scenario, check wheteher the model exist. If it does then save the model and delete it.
                if (incrementalUpdate == false)
                {
                    exportedModelContents = SaveExistingModelAndDelete(modelName, featureFolder, out exportedLobSystems);
                    upgrade = false;
                }
                else if (this.GetModel(modelName) == null)
                {
                    upgrade = false;
                }

                // Import the model file in the Bdc store.
                model = ImportModelFromFile(modelFilePath, PackageContents.Model |
                    PackageContents.Permissions | PackageContents.LocalizedNames | PackageContents.Properties, upgrade);

                if (resourceFilesPaths != null)
                {
                    // Import the resource model files in the Bdc store.
                    foreach (string resourceFile in resourceFilesPaths)
                    {
                        ImportModelFromFile(resourceFile, PackageContents.Permissions | PackageContents.LocalizedNames |
                            PackageContents.Properties, upgrade);
                    }
                }

                // import all the DLLs into the Bdc store.
                LobSystemCollection collection = model.AllReferencedLobSystems;
                LobSystemAssemblies lobassemblies;
                foreach (LobSystem lobSystem in collection)
                {
                    if (lobSystemAssemblies.TryGetValue(lobSystem.Name, out lobassemblies))
                    {
                        if (lobassemblies.MainAssembly != null)
                        {
                            lobSystem.PersistAssembly(lobassemblies.MainAssembly, lobassemblies.DependentAssemblies);
                        }
                    }
                }

                successful = true;
            }
            finally
            {
                // Recover only in case of ship.
                if (!successful && incrementalUpdate == false)
                {
                    // Delete the imported model if any of the above steps fails.
                    DeleteModel(model, false);
                    if (!string.IsNullOrEmpty(exportedModelContents))
                    {
                        //Model import failed, recovering previous model
                        ImportTheSavedModel(exportedModelContents, exportedLobSystems, incrementalUpdate);
                    }
                }
            }
        }

        /// <summary>
        /// Imports the model file in the Bdc store.
        /// </summary>
        /// <param name="filePath">Path of the model file.</param>
        /// <param name="packageContents">PackageContents to be imported.</param>
        /// <param name="incrementalUpdate">Flag indicating whether to update the existing model.</param>
        /// <returns>Model object corresponding to the model imported.</returns>
        private Model ImportModelFromFile(string filePath, PackageContents packageContents, bool incrementalUpdate)
        {
            string fileContents;
            using (StreamReader reader = new StreamReader(filePath))
            {
                fileContents = reader.ReadToEnd();
            }

            return ImportModel(fileContents, packageContents, incrementalUpdate);
        }

        /// <summary>
        /// Imports the model contents in the Bdc store.
        /// </summary>
        /// <param name="fileContents">Contents of the Model file.</param>
        /// <param name="packageContents">PackageContents to be imported.</param>
        /// <param name="incrementalUpdate">Flag indicating whether to update the existing model.</param>     
        /// <returns>Model object corresponding to the model imported.</returns>
        private Model ImportModel(string fileContents, PackageContents packageContents, bool incrementalUpdate)
        {
            string[] errors = null;
            Model model = null;

            model = this.amc.ImportPackage(fileContents, out errors, packageContents, null, incrementalUpdate, Guid.NewGuid());

            if (errors != null && errors.Length > 0)
            {
                StringBuilder errorMessage = new StringBuilder();
                //Start with a newline.
                errorMessage.AppendLine();
                foreach (string str in errors)
                {
                    errorMessage.AppendLine(str);
                }

                //Check to see if any of the entities in the model were not activated.
                //Entity.Validate() will return the reason why it happened. 
                List<Entity> allEntities = new List<Entity>(model.AllEntities);

                if (allEntities.Count > 0)
                {
                    StringBuilder activationErrors = new StringBuilder();
                    activationErrors.AppendLine();

                    ActivationError[] entityErrors = Entity.Validate(allEntities, this.amc);

                    foreach (ActivationError error in entityErrors)
                    {
                        activationErrors.AppendLine(error.ToString());
                    }

                    if (entityErrors.Length > 0)
                    {
                        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                                "Model file '{0}' has the following External Content Type activation errors:{1}",
                                this.modelFileName, activationErrors.ToString()), "ModelFileName");
                    }
                }
            }
            return model;
        }


        /// <summary>
        /// Finds the model by name.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <returns>true if the model exist else false.</returns>
        private Model GetModel(string modelName)
        {
            Model model = null;
            try
            {
                model = this.amc.GetModel(modelName);
            }
            catch (MetadataObjectNotFoundException)
            {
                return null;
            }
            return model;
        }

        /// <summary>
        /// Get the SPSite from the siteUrl property of the feature xml. If value is not present, use localhost as default.
        /// </summary>
        /// <param name="siteUrlProperty">SiteUrl property.</param>
        /// <returns></returns>
        private SPSite GetSite(SPFeatureProperty siteUrlProperty)
        {
            string url;
            if (null == siteUrlProperty || string.IsNullOrEmpty(siteUrlProperty.Value))
            {
                //Feature definition is missing 'SiteUrl' property, determining default webApp.
                url = GetDefaultWebAppUrl();
            }
            else
            {
                url = siteUrlProperty.Value;
            }

            SPSite site;
            try
            {
                site = new SPSite(url);
            }
            catch (FileNotFoundException e)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                    "Property 'SiteUrl' contains an invalid URL. Import failed with the following exception message: {0}", e.Message), "properties");
            }

            return site;
        }

        /// <summary>
        /// Gets the default webapp when siteURL property is not set.
        /// - Get all webapps (that are not admin) that are on port 80. Use the one with least number of dots (segments). When there are
        /// multiple with same number of dots, use the one starting with www. Otherwise fail.
        /// - If there are no webapps on port 80, do same as above for 443.
        /// </summary>
        /// <returns>WebApp URL</returns>
        private string GetDefaultWebAppUrl()
        {
            Uri result = null;
            bool conflict = true;
            SPWebServiceCollection webServices = new SPWebServiceCollection(this.parentFarm);

            foreach (SPWebService webService in webServices)
            {
                foreach (SPWebApplication webApp in webService.WebApplications)
                {
                    Uri uri = webApp.GetResponseUri(SPUrlZone.Default);

                    if (!webApp.IsAdministrationWebApplication && ((uri.Port == 80 && uri.Scheme.ToUpperInvariant().Equals("HTTP")) ||
                            (uri.Port == 443 && uri.Scheme.ToUpperInvariant().Equals("HTTPS"))))
                    {
                        if (result == null)
                        {
                            conflict = false;
                            result = uri;
                        }
                        else
                        {
                            string[] uriSegments = uri.Host.Split('.');
                            string[] resultSegments = result.Host.Split('.');

                            //Give preference to port 80.
                            if (result.Port == 80 && uri.Port == 443)
                            {
                                continue;
                            }
                            else if (result.Port == 443 && uri.Port == 80)
                            {
                                result = uri;
                                continue;
                            }

                            if (resultSegments.Length > uriSegments.Length)
                            {
                                conflict = false;
                                result = uri;
                            }
                            else if (resultSegments.Length == uriSegments.Length)
                            {
                                bool uriIsWww = uri.Segments[0].ToUpperInvariant().Equals("WWW");
                                bool resultIsWww = result.Segments[0].ToUpperInvariant().Equals("WWW");

                                if (uriIsWww && !resultIsWww)
                                {
                                    conflict = false;
                                    result = uri;
                                }
                                else if (!uriIsWww && resultIsWww)
                                {
                                    conflict = false;
                                }
                                else
                                {
                                    conflict = true;
                                }
                            }
                        }
                    }
                }
            }

            if (conflict)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                    "Feature {0} definition is missing 'SiteUrl' property and it cannot be determined.", this.featureName), "properties");
            }

            return result.OriginalString;
        }


        /// <summary>
        /// Returns the AdministrationMetadataCatalog from the Url of the Site or WebApplication.
        /// </summary>
        /// <param name="siteUrlProperty">SiteUrl property used to get the SPSite.</param>
        /// <returns>AdministrationMetadataCatalog object.</returns>
        private void CreateAdministrationMetadataCatalog(SPFeatureProperty siteUrlProperty)
        {

            SPServiceContext context = null;
            SPSite site = null;

            try
            {
                site = GetSite(siteUrlProperty);
                context = SPServiceContext.GetContext(site);
            }
            finally
            {
                if (site != null)
                {
                    site.Dispose();
                }
            }

            BdcService bdcService = this.parentFarm.Services.GetValue<BdcService>();
            if (bdcService == null)
            {
                throw new InvalidOperationException("Unable to contact BdcService.");
            }

            this.amc = bdcService.GetAdministrationMetadataCatalog(context);
            if (this.amc == null)
            {
                throw new InvalidOperationException("Unable to create AdministrationMetadataCatalog.");
            }
        }

        /// <summary>
        /// Deletes the specified Model.
        /// </summary>
        /// <param name="model">Model to be deleted.</param>
        /// <param name="deleteNoCascade">Flag indicating whether to Delete or todo DeleteNoCascadde.</param>
        private static void DeleteModel(Model model, bool deleteNoCascade)
        {
            if (null == model)
            {
                return;
            }

            string modelName = model.Name;
            if (deleteNoCascade)
            {
                EntityCollection ownedEntities = model.OwnedEntities;

                //Deactivate all owned entities.
                foreach (Entity entity in ownedEntities)
                {
                    entity.Deactivate();
                }

                model.DeleteNoCascade();
            }
            else
            {
                model.Delete();
            }
        }

        /// <summary>
        /// Converts the assembly to byte array.
        /// </summary>
        /// <param name="assemblyPath">Full path of the assembly to be converted.</param>
        /// <returns>byte[] of the assembly.</returns>
        private static byte[] GetAssemblyBytes(string assemblyPath)
        {
            if (string.IsNullOrEmpty(assemblyPath))
            {
                throw new ArgumentNullException("assemblyPath");
            }
            byte[] assemblyBytes = null;
            using (FileStream fs = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length < Int32.MaxValue)
                {
                    assemblyBytes = new byte[fs.Length];
                    assemblyBytes = File.ReadAllBytes(assemblyPath);
                }
                else
                {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                        "Assembly file '{0}' exceeds the maximum length of {1}.", assemblyPath, Int32.MaxValue));
                }
            }
            return assemblyBytes;
        }

        /// <summary>
        /// Parses a semicolon delimited string. If the string is null or empty then empty string array is returned.
        /// </summary>
        /// <param name="value">String to be parsed.</param>
        /// <returns>Array of string value.</returns>
        private static string[] ParseSemicolonDelimitedNames(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }
            string[] fileNames = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Trim();
            }
            return fileNames;
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
