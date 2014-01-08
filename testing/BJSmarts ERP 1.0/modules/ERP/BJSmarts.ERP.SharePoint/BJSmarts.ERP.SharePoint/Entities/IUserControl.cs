using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface IUserControl
    {
        void LoadingDropdownTable(String listname, String fieldname, DropDownList ddl, string sortfield, Boolean addInitialValue);
        void LoadingLookupOneToOne(String listname, DropDownList ddl, string showfield, string sortfield, string IDfield, Boolean addInitialValue, int intLCID);
        void LoadingRadioChoiceTableOneToOne(String listname, RadioButtonList rbtn, string showfield, string sortfield, string IDfield, Boolean addInitialValue, int intLCID);
        void LoadingChoiceTable(String listname, String fieldname, CheckBoxList chk);
        void LoadingRadioChoiceTable(String listname, String fieldname, RadioButtonList rbtn);
        void LoadingLookupTable(String listname, DropDownList ddl, string sortfield, Boolean addInitialValue);
        void LoadingBCSLookupTable(String listname, DropDownList ddl, string sortfield, Boolean addInitialValue);
        String GetItemText(SPListItem Item, String colName);
        String GetItemTextChoice(SPListItem Item, String colName);
        String GetItemTextDropdownlist(SPListItem Item, String colName);
        String GetItemTextExternalDataSP(String listname, SPListItem Item, String colName, String Idfield, String showfield);
        String GetItemTextDropdownlistExternal(SPListItem Item, String colName);
        String GetItemTextUser(SPListItem Item, String colName);
        int GetItemIndex(SPListItem Item, DropDownList ddl, String colName);
        int GetItemIndexBCSField(SPListItem Item, DropDownList ddl, String colName);
        void SetItemCheckedRadioOnetoOne(SPListItem Item, String fieldname, RadioButtonList rbtn);
        void SetItemCheckBox(SPListItem Item, String fieldname, CheckBox chkbox);
        void SetItemChecked(SPListItem Item, String fielname, CheckBoxList chk);
        void SetItemCheckedRadio(SPListItem Item, String fieldname, RadioButtonList rbtn);
        void SetExternalFieldValue(SPListItem item, string fieldInternalName, string newValue);
        string getLocalizedValue(string strInput, int intLCID, string resourceFilename);
    }
}
