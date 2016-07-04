"Change Set:		8480Morphic-rss.268Morphic-rss.268:Made SystemWindow create it's icons more consistently.Morphic-ar.266:UserDialogBoxMorph gets an optional position argument. The menu item 'About Squeak' is now filled in with proper information.Morphic-rss.267:Made SystemWindow initialize it's icons consistently."!MorphicModel subclass: #SystemWindow	instanceVariableNames: 'labelString stripes label closeBox collapseBox activeOnlyOnTop paneMorphs paneRects collapsedFrame fullFrame isCollapsed menuBox mustNotClose labelWidgetAllowance updatablePanes allowReframeHandles labelArea expandBox'	classVariableNames: 'CloseBoxImage CollapseBoxImage ExpandBoxImage MenuBoxImage TopWindow'	poolDictionaries: ''	category: 'Morphic-Windows'!!SystemWindow class methodsFor: 'initializing' stamp: 'rss 12/13/2009 17:46'!expandBoxImage	^ ExpandBoxImage ifNil: [ (Form	extent: 10@10	depth: 32	fromArray: #( 3875602689 3875602689 3875602689 3875602689 3875602689 3875602689 0 0 0 0 3875602689 0 0 0 0 4127260929 3877181721 3877181721 3875602689 0 3875602689 0 0 0 0 3875602689 3212869760 0 3875602689 3212869760 3875602689 0 0 0 0 3875602689 3212869760 0 3875602689 3212869760 3875602689 0 0 0 0 3875602689 3212869760 0 3875602689 3212869760 3875602689 4127260929 3875602689 3875602689 3875602689 3875602689 3212869760 0 3875602689 3212869760 0 3877181721 3212869760 3212869760 3212869760 3212869760 3212869760 0 3875602689 3212869760 0 3877181721 0 0 0 0 0 0 3875602689 3212869760 0 3875602689 3875602689 3875602689 3875602689 3875602689 3875602689 3875602689 3875602689 3212869760 0 0 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760)	offset: 0@0) ]! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:42'!confirm: aString orCancel: cancelBlock at: aPointOrNil	"UserDialogBoxMorph confirm: 'Do you like chocolate?'"	| dialog resp |	dialog := self new.	dialog title: 'Confirmation'.	dialog label: aString.	dialog addButton: '       Yes       ' translated value: true.	dialog addButton: '        No        ' translated  value: false..	dialog addButton: '     Cancel     ' translated  value: nil..	resp := dialog runModalIn: ActiveWorld forHand: ActiveHand at: aPointOrNil.	^resp ifNil:[cancelBlock value]! !!SystemWindow class methodsFor: 'initializing' stamp: 'rss 12/13/2009 17:48'!initialize	CollapseBoxImage := nil.	CloseBoxImage := nil.	ExpandBoxImage := nil.	MenuBoxImage := nil! !!SystemWindow methodsFor: 'initialization' stamp: 'rss 12/13/2009 17:39'!createExpandBox	^ self createBox 				labelGraphic: self class expandBoxImage;			 extent: self boxExtent;		 actWhen: #buttonUp;		 actionSelector: #expandBoxHit;		 setBalloonText: 'expand to full screen' translated! !!TheWorldMainDockingBar methodsFor: 'menu actions' stamp: 'ar 12/12/2009 18:43'!aboutSqueak	UserDialogBoxMorph		inform: SmalltalkImage current systemInformationString withCRs		title: 'About Squeak:'		at: World center.! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:42'!confirm: aString orCancel: cancelBlock	"UserDialogBoxMorph confirm: 'Do you like chocolate?'"	^self confirm: aString orCancel: cancelBlock at: nil! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:41'!confirm: aString title: titleString at: aPointOrNil	"UserDialogBoxMorph confirm: 'Make your choice carefully' withCRs title: 'Do you like chocolate?'"	| dialog |	dialog := self new.	dialog title: titleString.	dialog label: aString.	dialog addButton: '       Yes       ' translated value: true.	dialog addButton: '        No        ' translated  value: false..	^dialog runModalIn: ActiveWorld forHand: ActiveHand at: aPointOrNil! !!SystemWindow class methodsFor: 'initializing' stamp: 'rss 12/13/2009 17:49'!menuBoxImage	^ MenuBoxImage ifNil: [(Form	extent: 10@10	depth: 32	fromArray: #( 4227858432 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4227858432 0 4127195136 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 4127195136 3212869760 4127195136 3212869760 0 0 0 0 0 0 4127195136 3212869760 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 3212869760 4127195136 3212869760 0 0 0 0 0 0 4127195136 3212869760 4127195136 3212869760 0 0 0 0 0 0 4127195136 3212869760 4227858432 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 3212869760 4127195136 3212869760 0 0 0 0 0 0 4127195136 3212869760 4227858432 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4127195136 4227858432 3212869760 0 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760 3212869760)	offset: 0@0)]! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:42'!confirm: aString title: titleString	"UserDialogBoxMorph confirm: 'Make your choice carefully' withCRs title: 'Do you like chocolate?'"	^self confirm: aString title: titleString at: nil! !!UserDialogBoxMorph methodsFor: 'running' stamp: 'ar 12/12/2009 18:45'!runModalIn: aWorld forHand: aHand at: aPointOrNil	"Ensure that we have a reasonable minimum size"	| oldFocus pos |	(ProvideAnswerNotification signal: self label asString) ifNotNil:[:answer| ^answer].	self openInWorld: aWorld.	pos := (aPointOrNil ifNil:[aHand position]) - (self fullBounds extent // 2).	self setConstrainedPosition: pos hangOut: false.	oldFocus := aHand keyboardFocus.	aHand newMouseFocus: self.	aHand newKeyboardFocus: self.	[self isInWorld] whileTrue:[aWorld doOneSubCycle].	oldFocus ifNotNil:[aHand keyboardFocus: oldFocus].	^value! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:40'!inform: aString title: titleString at: aPointOrNil	"UserDialogBoxMorph inform: 'Squeak is great!!' title: 'Will you look at this:'"	| dialog |	dialog := self new.	dialog title: titleString.	dialog label: aString.	dialog addButton: '       OK       ' translated value: nil.	^dialog runModalIn: ActiveWorld forHand: ActiveHand at: aPointOrNil! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 12/12/2009 18:42'!inform: aString title: titleString	"UserDialogBoxMorph inform: 'Squeak is great!!' title: 'Will you look at this:'"	^self inform: aString title: titleString at: nil! !!SystemWindow methodsFor: 'initialization' stamp: 'rss 12/13/2009 17:41'!createMenuBox	^ self createBox 				labelGraphic: self class menuBoxImage;		 extent: self boxExtent;		 actWhen: #buttonDown;		 actionSelector: #offerWindowMenu;		 setBalloonText: 'window menu' translated! !UserDialogBoxMorph removeSelector: #runModalIn:forHand:!SystemWindow initialize!