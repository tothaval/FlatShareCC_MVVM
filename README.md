# SharedLivingCostCalculator dev_build

 primary use case:   
 calculation of costs and advances for people who share an office, 
 a flat or other areas. 
 
 secondary use case: 
 me learning and practicing MVVM

 Goals:
 
	primary goals:
	- the goal is to offer a flexible solution to the use case, which is able to perform the default task:
	calculating rent, fixed costs and heating costs for the rented object, for every room within it,
	for monthly payments or annual billings, as well as basing new heating costs advances on the
	consumption values from the previous annual billing.
	- the solution should be easy to use and understand, as well as forgiving on user error.
	- the solution should be easy to change in appearance, mainly to include people with perceptional difficulties
		or certain optical requirements
	
	to make it more flexible, the secondary goals are:
	- the solution should be able to create, edit and delete rent data, billing data and flat data.
	- the solution should be able to factor in credits and other costs for monthly or annual payments.
	- the solution should be able to generate a printable or copyable output, which can be printed for real
	or saved as pdf.
	
	to make it even more flexible, the tertiary goals are:
	- implement an assignement feature for rooms and tenants, integrate the assignements into
	the costs and print output, if workplaces are integrated, assignements should include workplaces per room somehow
	- implement a workplace feature, which offers the ability to assign a number of workplaces per room
	- implement an installment payment and ongoing costs/credits feature 
		
	to make it more robust:
	- implement a backup feature for user input data
	
	to make it cleaner:
	- refactor duplications of code wherever possible
	- comment the code, organize the code, remove obsolete code
	- KISS

current features: 
WIP	
		!!! the MainWindow has no WindowStyle and no Minimize, Maximize or CloseButton
		!!! to close the window right-click on the window, select close and confirm.
		!!! to minimize the window right-click on the window, select minimize.
		!!! to maximize the window double-left-click on the window near the outer border.
		!!! to normalize the maximized window double left-click on the window near the outer border.
		
		!!! the MainWindow will respond to dragging via left mouse pressed almost everywhere on its surface.
		
		the program now features a single Window with expandable areas, listviews and tabcontrols.
		
		togglebuttons and tabcontrol headers allow you to navigate by showing or hiding the associated areas
		they are styled as headers and bear header colors, header fontsize is 1.5 x fontsize
		
		normal buttons are styled with normal textcolor
		
		all buttons will switch their textcolor with the background color on mouseover or selection
		they will reverse the colors on deselection and mouseleave, they will switch colors and add
		some opacity if they became unenabled and will change back once enabled again.
		
!!! calculations are not properly tested as of yet, do not believe the results
!!! object and value interaction logic is not thoroughly checked as of yet

( all data regarding billing values must be considered false, because billingviewmodel storage and access
within the app is WIP atm. i am moving them out of a single rentviewmodel and into a separate list.

features:

- create, edit and delete Flats, Rooms, Rents, Billings, Payments, OtherCosts, Credits, Tenants, TenantConfigurations
- data will be saved and loaded, you can check the data in the binary folder, there will be folders created by the
	program and some loose xml files with numbers before them, those are the flat object files
- easy customizable appearance via Settings field, change language, culture(currency and date formats),
	fontsize, ui rounding and the four colors used throughout the application
- check manual for help WIP

the intended workflow is:

	in case a new shared flat or office is obtained:
	create a flat(or several, if needed) -> setup flat, rooms, tenants -> setup rent changes -> show costs tab
	
	in case an annual billing is received:
	-> create new billing in annual billings tab, fill in the values
	-> you can determine the complexity of the calculation, if there isn't any pending dispute or dire need to 
		know exactly, leave payments unchecked, the consequence is that rent costs are not factored into the
		annual billing calculation, in such cases, the program calculates advances under the assumption, that
		every one payed the calculated shares of the relevant rent changes
	-> click show costs tab
	
	in case some cost changes appear:
	-> create a new rent change or click 'add raise' and fill in or change the values -> click show costs tab
	
	in case some other changes appear:
	-> uncheck 'Is Active' on Tenant setup and insert a moving out date, currently no further effect
	-> add, edit or remove payments on annual billing tab
	-> a credit was received from the landlord due to some reason
	-> add a credit or other costs on Rent or Billing, select duration and hight of the sum
		-> the results are currently not calculated, still WIP