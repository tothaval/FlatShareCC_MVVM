# SharedLivingCostCalculator dev_build
 rebuild of FSCC using MVVM WPF pattern
 primary use case: me learning and practicing MVVM

current features:

-> create, edit, delete flat
	-> add new Flat Button -> enter FlatSetup View
	-> edit Flat Button -> enter FlatSetup View

	FlatSetup View (New Flat)
	-> setup flat with address, details, area, roomcount,
		depending on roomcount setup n times RoomArea & RoomName
	-> a new flat can have several rooms, if combined area
		of rooms exceeds flat area, an error is raised
	-> proceed Button -> return to Flatmanagement Overview page, create Flat
	-> leave Button -> return to Flatmanagement Overview page
	
	FlatSetup View (Edit)
	-> change Details or RoomNames
	-> leave Button -> return to Flatmanagement Overview page

-> select Flat
	-> Accounting Button -> enter Accounting View
		
	Accounting View
		Tab Rent -> create, edit, delete Rent data, show Costs Button -> Costs Window
		Tab Billing -> create, edit, delete BillingPeriod data, show Costs Button -> Costs Window
		Tab Payments -> select room 
			-> create, edit, delete Payments data per room
			-> setup n payments per room				
			-> payments can be setup as ranges with quantity and enddate

	-> leave Button -> return to Flatmanagement Overview page

-> Costs
	-> Flatmanagement overview page will show Costs data of selected flat
	-> the newest Rent data will be used for calculation of currentRent
	
-> Costs Window
	-> displays Costs for Rent and for Billing
		-> Rent shows monthly and annual costs based on rent values
		-> Billing shows billing related costs
		
	-> !!! beware of results, calculations haven't been thoroughly tested yet	
	-> !!! new rent values are not yet based on previous consumption values of billing,
			the results are based on area ratio or false values since linking between
			objects is not yet fully implemented

-> Settings
	->show Settings window
	-> Background, Foreground, FontFamily and FontSize can be set


-> data will be saved and loaded
-> settings will be saved and loaded
