# SharedLivingCostCalculator dev_build

 primary use case:  
 
 calculation of costs and advances 
 for people who share an office, a
 flat or other areas.
					

 secondary use case:
 
 me learning and practicing MVVM


current features: 

WIP	
		!!! the MainWindow has no WindowStyle and no Minimize, Maximize or CloseButton
		!!! to close the window right-click on the window, select close and confirm.
		!!! to minimize the window right-click on the window, select minimize.
		!!! to maximize the window double-left-click on the window near the outer border.
		!!! to normalize the maximized window double left-click on the window near the outer border.
		
		!!! the MainWindow will respond to dragging everywhere at its outer regions
		!!! next to the resize areas.
		
		the program now features a single Window with expandable areas, as well as two 
		windows, one for billing data, and one for other costs.
		
		togglebuttons allow you to navigate by expanding or collapsing the associated areas
		they are styled as headers and bear header colors
		
		normal buttons are styled with normal textcolor
		
		all buttons will switch their textcolor with the background color on mouseover or selection
		they will reverse the colors on deselection and mouseleave
		
!!! calculations are not properly tested as of yet, do not believe the results
!!! object and value interaction logic is not thoroughly checked as of yet

features:

- create, edit and delete Flats, Rooms, Rents, Billings, Payments, OtherCosts, Tenants, TenantConfigurations
- data will be saved and loaded, except for TenantConfigurations as of yet, you can check the
	data in the binary folder, there will be folders created by the program and some loose xml files
- easy customize appearance via Settings field, change language, culture(currency and date formats),
	fontsize, ui rounding and the four colors used throughout the application
- check manual for help WIP

the intended workflow is:

	in case a new shared flat or office is obtained:
	create a flat(or several, if needed) -> setup flat, rooms, tenants -> setup rent changes -> show costs
	
	in case an annual billing is received:
	-> create new rent change, activate billing in rent options, fill in the values into the billing window
	-> you can determine the complexity of the calculation, if there isn't any pending dispute or dire need to 
		know exactly, leave payments unchecked, the consequence is that rent costs are not factored into the
		annual billing calculation
	-> close window -> show costs
	
	in case some cost changes appear:
	-> create a new rent change, fill in the values -> show costs
	
	in case some other changes appear:
	-> uncheck 'Is Active' on Tenant setup and insert a moving out date, currently no further effect
	-> add, edit or remove payments on billing window
	-> a credit was received from the landlord due to some reason -> add a credit on Rent or Billing (currently not supported)


