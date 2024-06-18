# SharedLivingCostCalculator dev_build
 rebuild of FSCC using MVVM WPF pattern
 primary use case:  calculation of costs and advances 
					for people who share an office, a
					flat or other areas.
					
					me learning and practicing MVVM

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
		WIP


	-> leave Button -> return to Flatmanagement Overview page

-> Costs
	-> Flatmanagement overview page will show Costs data of selected flat
	-> the newest Rent data will be used for calculation of currentRent
	
-> Costs Window
	-> displays Costs for Rent and for Billing
		-> Rent shows monthly and annual costs based on rent values
		-> Billing shows billing related costs
		
	-> !!! beware of results, calculations haven't been thoroughly tested yet

-> Settings
	->show Settings window
	-> Background, Foreground, FontFamily and FontSize can be set
	-> partially supports second language


-> data will be saved and loaded
-> settings will be saved and loaded

