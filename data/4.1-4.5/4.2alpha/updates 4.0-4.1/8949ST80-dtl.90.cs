"Change Set:		8949ST80-dtl.90ST80-dtl.90:Merge ST80-dtl.89 and ST80-ul.89ST80-ar.83:Replace offerMenuFrom:shifted: by offerMenu:from:shifted: which takes an additional argument, the model to retrieve the menu from and perform the actions on.ST80-mha.84:typo fixST80-ar.85:Allow changing the text of a PluggableText from the model using 'self changed: #editString with: aString'.ST80-ar.86:Merging ST80-nice.80:Experimental: let a Rectangle merge in place (I called this swallow:)This has two advantages:- avoid repeated Object creation when we just want the gross result- avoid closures writing to outer tempsIMHO, generalizing this kind of policy could have a measurable impact on GUI speed.However, this is against current policy to never change a Point nor rectangle in place, so I let gurus judge if worth or not.ST80-dtl.87:Remove all MVC BitEditor references from non-MVC packages.  Form>>bitEdit to Project class>>bitEdit:  Form>>bitEditAt:scale: to Project class>>bitEdit:at:scale:  BitEditor class>>locateMagnifiedView:scale: to Rectangle class>>locateMagnifiedView:scale:ST80-dtl.88:Remove remaining dependencies on ST80-Editors from non-MVC packages.Remove explicit references to ST80 classes from ModalSystemWindow and various utility methods.ST80-ul.89:- fix: ParagraphEditor >> #tallyItST80-dtl.89:Move PopUpMenu from ST80-Menus to Tools-Menus.Remove explicit MVC and Morphic dependencies from PopUpMenu.Support Project>>dispatchTo:addPrefixAndSend:withArguments:Add PopUpMenu>>mvcStartUpLeftFlushAdd PopUpMenu>>mvcStartUpWithCaption:icon:at:allowKeyboard:"!!MVCProject methodsFor: 'utilities' stamp: 'ar 1/10/2010 10:23'!offerMenu: menuSelector from: aModel shifted: aBoolean	"Pop up a menu whose target is aModel and whose contents are provided	by sending the menuSelector to the model. The menuSelector takes two	arguments: a menu, and a boolean representing the shift state."	| aMenu |	aMenu := CustomMenu new.	aModel perform: menuSelector with: aMenu with: aBoolean.	aMenu invokeOn: aModel! !!PopUpMenu methodsFor: '*ST80-Menus' stamp: 'dtl 1/30/2010 15:56'!mvcStartUpWithCaption: captionOrNil icon: aForm at: location allowKeyboard: aBoolean	"Display the menu, with caption if supplied. Wait for the mouse button to go down, then track the selection as long as the button is pressed. When the button is released,	Answer the index of the current selection, or zero if the mouse is not released over  any menu item. Location specifies the desired topLeft of the menu body rectangle. The final argument indicates whether the menu should seize the keyboard focus in order to allow the user to navigate it via the keyboard."	frame ifNil: [self computeForm].	Cursor normal showWhile:		[self			displayAt: location			withCaption: captionOrNil			during: [self controlActivity]].	^ selection! !!MVCProject methodsFor: 'editors' stamp: 'dtl 1/24/2010 16:06'!formEdit: aForm	"Start up an instance of the FormEditor on a form. Typically the form 	is not visible on the screen. The editor menu is located at the bottom of 	the form editing frame. The form is displayed centered in the frame. 	YellowButtonMenu accept is used to modify the form to reflect the 	changes made on the screen version; cancel restores the original form to 	the screen. Note that the changes are clipped to the original size of the 	form." 	FormEditor openOnForm: aForm! !!View methodsFor: 'window access' stamp: 'nice 12/27/2009 22:39'!defaultWindow	"Build the minimum Rectangle that encloses all the windows of the 	receiver's subViews. The answer is a Rectangle obtained by expanding 	this minimal Rectangle by the borderWidth of the receiver. If the 	receiver has no subViews, then a Rectangle enclosing the entire display 	screen is answered. It is used internally by View methods if no window 	has been specified for the View. Specialized subclasses of View should 	redefine View|defaultWindow to handle the default case for instances 	that have no subViews."	| aRectangle |	subViews isEmpty ifTrue: [^DisplayScreen boundingBox].	aRectangle := self firstSubView viewport copy.	subViews do: [:aView | aRectangle swallow: aView viewport].	^aRectangle expandBy: borderWidth! !!BitEditor class methodsFor: 'instance creation' stamp: 'dtl 1/23/2010 18:13'!openOnForm: aForm 	"Create and schedule a BitEditor on the form aForm at its top left corner. 	Show the small and magnified view of aForm."	| scaleFactor |	scaleFactor := 8 @ 8.	^self openOnForm: aForm		at: (Rectangle locateMagnifiedView: aForm scale: scaleFactor) topLeft		scale: scaleFactor! !!ParagraphEditor methodsFor: 'do-its' stamp: 'ul 2/1/2010 01:41'!tallySelection	"Treat the current selection as an expression; evaluate it and return the time took for this evaluation"	| result rcvr ctxt valueAsString v |	self lineSelectAndEmptyCheck: [^ -1].	(model respondsTo: #doItReceiver) 		ifTrue: [FakeClassPool adopt: model selectedClass.  "Include model pool vars if any"				rcvr := model doItReceiver.				ctxt := model doItContext]		ifFalse: [rcvr := ctxt := nil].	result := [ | cm |		cm := rcvr class evaluatorClass new 			compiledMethodFor: self selectionAsStream			in: ctxt			to: rcvr			notifying: self			ifFail: [FakeClassPool adopt: nil. ^ #failedDoit]			logged: false.		Time millisecondsToRun: 			[v := cm valueWithReceiver: rcvr arguments: #() ].	] 		on: OutOfScopeNotification 		do: [ :ex | ex resume: true].	FakeClassPool adopt: nil.	"We do not want to have large result displayed"	valueAsString := v printString.	(valueAsString size > 30) ifTrue: [valueAsString := (valueAsString copyFrom: 1 to: 30), '...'].	PopUpMenu 		inform: 'Time to compile and execute: ', result printString, 'ms res: ', valueAsString.! !!FormEditor methodsFor: 'editing tools' stamp: 'dtl 1/23/2010 18:12'!magnify	"Allow for bit editing of an area of the Form. The user designates a 	rectangular area that is scaled by 5 to allow individual screens dots to be 	modified. Red button is used to set a bit to black, and yellow button is 	used to set a bit to white. Editing continues until the user depresses any 	key on the keyboard."	| smallRect smallForm scaleFactor tempRect |	scaleFactor := 8@8.	smallRect := (Rectangle fromUser: grid) intersect: view insetDisplayBox.	smallRect isNil ifTrue: [^self].	smallForm := Form fromDisplay: smallRect.	"Do this computation here in order to be able to save the existing display screen."	tempRect := Rectangle locateMagnifiedView: smallForm scale: scaleFactor.	BitEditor		openScreenViewOnForm: smallForm 		at: smallRect topLeft 		magnifiedAt: tempRect topLeft 		scale: scaleFactor.	tool := previousTool! !!SelectionMenu class methodsFor: 'instance creation' stamp: 'mha 1/18/2010 10:13'!labelList: labelList lines: lines	^ self labelArray: labelList lines: lines! !!MVCProject methodsFor: 'editors' stamp: 'dtl 1/23/2010 18:34'!bitEdit: aForm at: magnifiedFormLocation scale: scaleFactor	"Create and schedule a view whose top left corner is magnifiedLocation 	and that contains a view of aForm magnified by scaleFactor that  can be	modified using the Bit Editor. It also contains a view of the original form."	BitEditor openOnForm: aForm at: magnifiedFormLocation scale: scaleFactor ! !!BitEditor class methodsFor: 'examples' stamp: 'dtl 1/23/2010 18:13'!magnifyOnScreen	"Bit editing of an area of the display screen. User designates a 	rectangular area that is magnified by 8 to allow individual screens dots to	be modified. red button is used to set a bit to black and yellow button is	used to set a bit to white. Editor is not scheduled in a view. Original	screen location is updated immediately. This is the same as FormEditor	magnify."	| smallRect smallForm scaleFactor tempRect |	scaleFactor := 8 @ 8.	smallRect := Rectangle fromUser.	smallRect isNil ifTrue: [^self].	smallForm := Form fromDisplay: smallRect.	tempRect := Rectangle locateMagnifiedView: smallForm scale: scaleFactor.	"show magnified form size until mouse is depressed"	self		openScreenViewOnForm: smallForm 		at: smallRect topLeft 		magnifiedAt: tempRect topLeft 		scale: scaleFactor	"BitEditor magnifyOnScreen."! !!MVCProject methodsFor: 'editors' stamp: 'dtl 1/23/2010 18:25'!bitEdit: aForm	"Create and schedule a view located in an area designated by the user 	that contains a view of aForm magnified by 8@8 that can be modified using	a bit editor. It also contains a view of the original form."	BitEditor openOnForm: aForm	"Note that using direct messages to BitEditor, fixed locations and scales can be created.	That is, also try:		BitEditor openOnForm: self at: <some point>		BitEditor openOnForm: self at: <some point> scale: <some point>"! !!View methodsFor: 'display box access' stamp: 'nice 12/27/2009 22:39'!computeBoundingBox	"Answer the minimum Rectangle that encloses the bounding boxes of the 	receiver's subViews. If the receiver has no subViews, then the bounding 	box is the receiver's window. Subclasses should redefine 	View|boundingBox if a more suitable default for the case of no subViews 	is available."	| aRectangle |	subViews isEmpty ifTrue: [^self getWindow].	aRectangle := (self firstSubView transform: self firstSubView boundingBox) copy.	subViews do: 		[:aView | 		aRectangle swallow: (aView transform: aView boundingBox).].	^aRectangle expandBy: borderWidth! !!MVCProject methodsFor: 'editors' stamp: 'dtl 1/24/2010 11:30'!editCharacter: character ofFont: strikeFont	"Open a bit editor on a character in the given strike font. Note that you must	do an accept (in the option menu of the bit editor) if you want this work. 	Accepted edits will not take effect in the font until you leave or close the bit editor. 	Also note that unaccepted edits will be lost when you leave or close."	"Note that BitEditor only works in MVC currently."	"(TextStyle default fontAt: 1) edit: $="	| charForm editRect scaleFactor bitEditor savedForm r |	charForm := strikeFont characterFormAt: character.	editRect := Rectangle locateMagnifiedView: charForm scale: (scaleFactor := 8 @ 8).	bitEditor := BitEditor				bitEdit: charForm				at: editRect topLeft				scale: scaleFactor				remoteView: nil.	savedForm := Form fromDisplay: (r := bitEditor displayBox							expandBy: (0 @ 23 corner: 0 @ 0)).	bitEditor controller startUp.	bitEditor release.	savedForm displayOn: Display at: r topLeft.	strikeFont characterFormAt: character put: charForm! !!PopUpMenu methodsFor: '*ST80-Menus' stamp: 'dtl 1/30/2010 16:14'!mvcStartUpLeftFlush	"Build and invoke this menu with no initial selection.  By Jerry Archibald, 4/01.	If in MVC, align menus items with the left margin.	Answer the selection associated with the menu item chosen by the user or nil if none is chosen.  	The mechanism for getting left-flush appearance in mvc leaves a tiny possibility for misadventure: if the user, in mvc, puts up the jump-to-project menu, then hits cmd period while it is up, then puts up a second jump-to-project menu before dismissing or proceeding through the debugger, it's possible for mvc popup-menus thereafter to appear left-aligned rather than centered; this very unlikely condition can be cleared by evaluating 'PopUpMenu alignment: 2'"	| saveAlignment |	saveAlignment := PopUpMenu alignment.		PopUpMenu leftFlush.	^[self startUp] ensure:		[PopUpMenu alignment: saveAlignment]! !!MVCProject methodsFor: 'dispatching' stamp: 'dtl 1/30/2010 15:49'!selectorPrefixForDispatch	"A string to be prepended to selectors for project specific methods"	^ 'mvc'! !!PluggableTextView methodsFor: 'updating' stamp: 'ar 1/19/2010 20:28'!update: aSymbol with: arg1	aSymbol == #editString ifTrue:[		self editString: arg1.		^self hasUnacceptedEdits: true.	].	^super update: aSymbol with: arg1! !!Path methodsFor: 'display box access' stamp: 'nice 12/27/2009 22:38'!computeBoundingBox 	"Refer to the comment in DisplayObject|computeBoundingBox."	| box |	box := Rectangle origin: (self at: 1) extent: 0 @ 0.	collectionOfPoints do: 		[:aPoint | box swallow: (Rectangle origin: aPoint extent: 0 @ 0)].	^box! !PopUpMenu removeSelector: #startUpWithCaption:icon:at:!PopUpMenu class removeSelector: #initialize!PopUpMenu class removeSelector: #withCaption:chooseFrom:!PopUpMenu removeSelector: #markerTop:!PopUpMenu removeSelector: #computeLabelParagraph!PopUpMenu removeSelector: #startUpSegmented:withCaption:at:allowKeyboard:!PopUpMenu class removeSelector: #confirm:orCancel:!PopUpMenu class removeSelector: #alignment!PopUpMenu removeSelector: #center!PopUpMenu class removeSelector: #labels:lines:!PopUpMenu removeSelector: #nItems!PopUpMenu class removeSelector: #notify:!PopUpMenu removeSelector: #selection!PopUpMenu removeSelector: #frameHeight!PopUpMenu removeSelector: #labels:font:lines:!PopUpMenu removeSelector: #startUpWithCaption:at:allowKeyboard:!PopUpMenu class removeSelector: #labels:!PopUpMenu removeSelector: #startUpWithoutKeyboard!Smalltalk removeClassNamed: #PopUpMenu!MVCProject removeSelector: #offerMenuFrom:shifted:!PopUpMenu removeSelector: #setSelection:!PopUpMenu class removeSelector: #confirm:!PopUpMenu removeSelector: #startUpWithCaption:at:!PopUpMenu removeSelector: #startUpWithCaption:!PopUpMenu removeSelector: #startUpCenteredWithCaption:!PopUpMenu class removeSelector: #labelArray:!PopUpMenu removeSelector: #rescan!BitEditor class removeSelector: #locateMagnifiedView:scale:!PopUpMenu removeSelector: #startUp!PopUpMenu removeSelector: #markerOn:!PopUpMenu removeSelector: #startUpWithCaption:icon:at:allowKeyboard:!PopUpMenu removeSelector: #lineArray!PopUpMenu removeSelector: #menuForm!PopUpMenu removeSelector: #computeForm!PopUpMenu class removeSelector: #confirm:trueChoice:falseChoice:!PopUpMenu removeSelector: #displayAt:withCaption:during:!PopUpMenu removeSelector: #startUpWithCaption:icon:!PopUpMenu removeSelector: #labelString!PopUpMenu class removeSelector: #inform:!PopUpMenu class removeSelector: #labelArray:lines:!PopUpMenu removeSelector: #startUpSegmented:withCaption:at:!PopUpMenu removeSelector: #controlActivity!PopUpMenu removeSelector: #startUpLeftFlush!PopUpMenu removeSelector: #markerOff!PopUpMenu class removeSelector: #leftFlush!PopUpMenu removeSelector: #scrollIntoView:!PopUpMenu class removeSelector: #alignment:!PopUpMenu removeSelector: #manageMarker!PopUpMenu removeSelector: #readKeyboard!PopUpMenu class removeSelector: #setMenuFontTo:!PopUpMenu initialize!