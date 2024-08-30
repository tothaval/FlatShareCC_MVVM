using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Utility.LanguageRessources
{
    public class PopupHintStrings
    {

        #region flatmanagement view

        // Abbr.
        // LRS  Language Resource Strings
        // PH   Popup Hint
        // IDF  Identifier

        public string LRS_PH_Address { get; set; } = "Insert full adress of flat."; 
        public string LRS_PH_Deposit { get; set; } = "Enter the deposit for the contract, if there is any.";
        public string LRS_PH_Details { get; set; } = "Insert details to better describe\nthe flat or contract, if necessary.";
        public string LRS_PH_FlatArea { get; set; } = "Insert complete area of flat.";
        public string LRS_PH_RoomCount { get; set; } = "Insert the number of rooms.";
        public string LRS_PH_UseRooms { get; set; } = "Select, if you wish to calculate values based on rooms.";
        public string LRS_PH_UseWorkplaces { get; set; } = "Select, if you intend to calculate values based on workplaces.";
        public string LRS_PH_FlatNotes { get; set; } = "Insert notes on flat here, if there are any,\nto better describe flat, environment or contract or else.";

        public string LRS_PH_UseFlatCosts { get; set; } = "Select, if you wish to calculate initial costs\nusing rent and an advance per flat.";
        public string LRS_PH_UseRoomCosts { get; set; } = "Select, if you wish to calculate initial costs\nusing rent and an advance per room.";
        public string LRS_PH_ContractStart { get; set; } = "Insert begin of rent contract here. Other dates can not begin before this date.";
        public string LRS_PH_Rent { get; set; } = "Insert monthly cold rent.";
        public string LRS_PH_Advance { get; set; } = "Insert monthly advance.";
        
        public string LRS_PH_RoomSetupAreaDisplay { get; set; } = 
            "Displays the sum of all rooms combined versus total flat area.\n\n" +
            "Shared non room area will be divided equally amongst rooms.\n" +
            "Shared area share plus room area will be used to calculate\n" +
            "most of the costs for each room.";
        public string LRS_PH_RoomName { get; set; } = "Insert the name of the room.";
        public string LRS_PH_RoomArea { get; set; } = "Insert the area of the room.";

        #endregion

        public PopupHintStrings()
        {

        }


    }
}
