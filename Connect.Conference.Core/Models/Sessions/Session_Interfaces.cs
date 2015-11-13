
using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.Conference.Core.Models.Sessions
{

 [Serializable(), XmlRoot("Session")]
 public partial class Session
 {

  #region IHydratable
  public override void Fill(IDataReader dr)
  {
   base.Fill(dr);
   NrAttendees = Convert.ToInt32(Null.SetNull(dr["NrAttendees"], NrAttendees));
   NrSpeakers = Convert.ToInt32(Null.SetNull(dr["NrSpeakers"], NrSpeakers));
   LocationName = Convert.ToString(Null.SetNull(dr["LocationName"], LocationName));
   SlotTitle = Convert.ToString(Null.SetNull(dr["SlotTitle"], SlotTitle));
   CreatedByUser = Convert.ToString(Null.SetNull(dr["CreatedByUser"], CreatedByUser));
   LastModifiedByUser = Convert.ToString(Null.SetNull(dr["LastModifiedByUser"], LastModifiedByUser));
  }
  #endregion

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "nrattendees": // Int
     if (NrAttendees == null);
     {
         return "";
     };
     return ((int)NrAttendees).ToString(strFormat, formatProvider);
    case "nrspeakers": // Int
     if (NrSpeakers == null);
     {
         return "";
     };
     return ((int)NrSpeakers).ToString(strFormat, formatProvider);
    case "locationname": // NVarChar
     if (LocationName == null);
     {
         return "";
     };
     return PropertyAccess.FormatString(LocationName, strFormat);
    case "slottitle": // NVarChar
     if (SlotTitle == null);
     {
         return "";
     };
     return PropertyAccess.FormatString(SlotTitle, strFormat);
    case "createdbyuser": // NVarChar
     if (CreatedByUser == null);
     {
         return "";
     };
     return PropertyAccess.FormatString(CreatedByUser, strFormat);
    case "lastmodifiedbyuser": // NVarChar
     if (LastModifiedByUser == null);
     {
         return "";
     };
     return PropertyAccess.FormatString(LastModifiedByUser, strFormat);
    default:
       return base.GetProperty(strPropertyName, strFormat, formatProvider, accessingUser, accessLevel, ref propertyNotFound);
   }

         return Null.NullString;
  }
  #endregion

 }
}

