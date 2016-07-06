'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:24 am'!StringMorph subclass: #BorderedStringMorph	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'MorphicExtras-Widgets'!!BorderedStringMorph methodsFor: 'drawing' stamp: 'ar 12/31/2001 02:34'!drawOn: aCanvas	| nameForm |	font _ self fontToUse.	nameForm _ Form extent: bounds extent depth: 8.	nameForm getCanvas drawString: contents at: 0@0 font: self fontToUse color: Color black.	(bounds origin + 1) eightNeighbors do: [ :pt |		aCanvas			stencil: nameForm 			at: pt			color: self borderColor.	].	aCanvas		stencil: nameForm 		at: bounds origin + 1 		color: color.	! !!BorderedStringMorph methodsFor: 'initialization' stamp: 'ar 12/14/2001 20:02'!initWithContents: aString font: aFont emphasis: emphasisCode	super initWithContents: aString font: aFont emphasis: emphasisCode.	self borderStyle: (SimpleBorder width: 1 color: Color white).! !!BorderedStringMorph methodsFor: 'initialization' stamp: 'dgd 2/14/2003 20:42'!initialize"initialize the state of the receiver"	super initialize.""	self		borderStyle: (SimpleBorder width: 1 color: Color white)! !!BorderedStringMorph methodsFor: 'accessing' stamp: 'ar 12/12/2001 03:03'!measureContents	^super measureContents +2.! !!ByteString methodsFor: 'converting' stamp: 'edc 11/19/2007 10:49'!asJoliet|badChars |badChars := #( $: $< $> $| $/ $\ $? $* $") asSet.^ self copyWithoutAll: badChars! !!ChangeSet methodsFor: 'fileIn/Out' stamp: 'edc 5/15/2005 12:11'!fileOutCompressed	"File out the receiver, to a file whose name is a function of the  	change-set name and either of the date & time or chosen to have a  	unique numeric tag, depending on the preference  	'changeSetVersionNumbers'"	| slips nameToUse internalStream shortnameToUse |	self checkForConversionMethods.	ChangeSet promptForDefaultChangeSetDirectoryIfNecessary.	nameToUse := Preferences changeSetVersionNumbers				ifTrue: [self defaultChangeSetDirectory nextNameFor: self name extension: FileStream cs]				ifFalse: [self name , FileDirectory dot , Utilities dateTimeSuffix, FileDirectory dot , FileStream cs].	(Preferences warningForMacOSFileNameLength			and: [nameToUse size > 30])		ifTrue: [nameToUse := FillInTheBlank						request: (nameToUse , '\has ' , nameToUse size asString , ' letters - too long for Mac OS.\Suggested replacement is:') withCRs						initialAnswer: (nameToUse contractTo: 30).			nameToUse = ''				ifTrue: [^ self]].shortnameToUse _ nameToUse.	nameToUse := self defaultChangeSetDirectory fullNameFor: nameToUse.	Cursor write showWhile: [			internalStream := WriteStream on: (String new: 10000).			internalStream header; timeStamp.			self fileOutPreambleOn: internalStream.			self fileOutOn: internalStream.			self fileOutPostscriptOn: internalStream.			internalStream trailer.			FileStream writeSourceCodeFrom: internalStream baseName: (nameToUse copyFrom: 1 to: nameToUse size - 3) isSt: false useHtml: false.	].	Preferences checkForSlips		ifFalse: [^ self].	slips := self checkForSlips.	(slips size > 0			and: [(PopUpMenu withCaption: 'Methods in this fileOut have haltsor references to the Transcriptor other ''slips'' in them.Would you like to browse them?' chooseFrom: 'Ignore\Browse slips')					= 2])		ifTrue: [self systemNavigation browseMessageList: slips name: 'Possible slips in ' , name].CodeLoader compressFileNamed: shortnameToUse in: self defaultChangeSetDirectory! !!DualChangeSorter methodsFor: 'initialization' stamp: 'edc 7/16/2005 10:46'!morphicWindow		| window |		leftCngSorter _ ChangeSorter new myChangeSet: ChangeSet current.	leftCngSorter parent: self.	rightCngSorter _ ChangeSorter new myChangeSet: 			ChangeSorter secondaryChangeSet.	rightCngSorter parent: self.	window _ (SystemWindow labelled: leftCngSorter label) model: self.	"topView minimumSize: 300 @ 200."	leftCngSorter openAsMorphIn: window rect: (0@0 extent: 0.5@1).	rightCngSorter openAsMorphIn: window rect: (0.5@0 extent: 0.5@1).	^ window! !!MCConfiguration methodsFor: 'private' stamp: 'edc 3/8/2010 10:52'!depsSatisfying: selectBlock versionDo: verBlock displayingProgress: progressString 	| repoMap count packagesInThis existe |	repoMap := Dictionary new.	packagesInThis := #('311Deprecated' 'Balloon' 'Collections' 'Compiler' 'Compression' 'Exceptions' 'Files' 'Graphics' 'Kernel' 'MinimalMorphic'  'Morphic' 'MorphicExtras' 'Multilingual' 'Network' 'PackageInfo-Base' 'ST80' 'SUnit' 'Services' 'ShoutCore' 'Sound' 'Squeak-Version' 'System' 'ToolBuilder-Kernel' 'ToolBuilder-MVC' 'ToolBuilder-Morphic' 'ToolBuilder-SUnit' 'Tools' 'Traits' 'TraitsTests' 'TrueType'). "this is needed until we agree on the polished changes go to trunk and become the default to all." .	self repositories		do: [:repo | 			MCRepositoryGroup default addRepository: repo.			repo allVersionNames				ifEmpty: [self logWarning: 'cannot read from ' , repo description]				ifNotEmptyDo: [:all | all						do: [:ver | repoMap at: ver put: repo]]].	count := 0.	self dependencies		do: [:dep | 			| ver repo | 			existe := packagesInThis includes: dep package name.			existe				ifTrue: [ver := dep versionInfo name.					repo := repoMap								at: ver								ifAbsent: [self logError: 'Version ' , ver , ' not found in any repository'.									self logError: 'Aborting'.									^ count].					(selectBlock value: dep)						ifTrue: [| new | 							new := self										versionNamed: ver										for: dep										from: repo.							new								ifNil: [self logError: 'Could not download version ' , ver , ' from ' , repo description.									self logError: 'Aborting'.									^ count]								ifNotNil: [self logUpdate: dep package with: new.									ProgressNotification signal: '' extra: 'Installing ' , ver.									verBlock value: new.									count := count + 1]].					dep package workingCopy repositoryGroup addRepository: repo]				ifFalse: [self logWarning: 'NOT LOADING ' , dep package name printString]]		displayingProgress: progressString.	^ count! !!MailComposition methodsFor: 'interface' stamp: 'edc 11/19/2007 10:53'!saveContentsInFile	"Save the receiver's contents string to a file, prompting the user for a file-name.  Suggest a reasonable file-name."	| fileName stringToSave suggestedName |	stringToSave := (RWBinaryOrTextStream with: messageText  string) reset.	suggestedName := stringToSave  upToAll: 'Subject: ';nextLine.				suggestedName := (suggestedName, '.text' ) asJoliet.						fileName := UIManager default request: 'File name?' translated			initialAnswer: suggestedName.	fileName isEmptyOrNil ifFalse:		[(FileStream newFileNamed: fileName) nextPutAll: stringToSave reset; close]! !!MethodReference methodsFor: 'string version' stamp: 'edc 5/23/2007 09:32'!stringVersionstringVersion ifNil: [ stringVersion := self actualClass name, ' >> ', methodSymbol].	^stringVersion! !!Object class methodsFor: 'objects from disk' stamp: 'edc 6/11/2008 07:37'!readAndInspect: inputStreaminputStream setConverterForCode.(inputStream fileInObjectAndCode ) inspect! !!Object class methodsFor: '*services-extras' stamp: 'edc 2/14/2008 08:24'!fileReaderServicesForFile: fullName suffix: suffix	| services |	services _ OrderedCollection new.		(fullName asLowercase endsWith: '.obj')		ifTrue: [ services add: self serviceLoadObject ].	^services! !!Object class methodsFor: '*services-extras' stamp: 'edc 10/25/2006 17:45'!registeredServices
	^ { 
	Service new
		label: 'Open saved objects';
		shortLabel: 'object'; 
		description: 'load back saved object ';
		action: [:stream | self readAndInspect: (FileStream oldFileOrNoneNamed:stream name)];
		shortcut: nil;
		categories: Service worldServiceCat.} ! !!Object class methodsFor: '*services-extras' stamp: 'edc 2/14/2008 08:26'!serviceLoadObject"Answer a service for opening a saved Object"	^ (SimpleServiceEntry 		provider: self 		label: 'saved Object'		selector: #readAndInspect:		description: 'open a Object'		buttonLabel: 'object')		argumentGetter: [:fileList | fileList readOnlyStream]! !!ChangesOrganizer class methodsFor: '*MinimalMorphic-services' stamp: 'edc 3/6/2010 11:33'!reorderChangeSets	"Change the order of the change sets to something more convenient:	First come the project changesets that come with the release. These are	mostly empty.	Next come all numbered updates.	Next come all remaining changesets	In a ChangeSorter, they will appear in the reversed order."	"ChangeSorter reorderChangeSets"	| newHead newMid newTail |	newHead := OrderedCollection new.	newTail := OrderedCollection new.	newMid := SortedCollection				sortBlock: [:x :y | x printString > y printString].	ChangeSet allChangeSets		do: [:aChangeSet | (self belongsInProjectsInRelease: aChangeSet)				ifTrue: [newHead add: aChangeSet]				ifFalse: [(self belongsInNumbered: aChangeSet)						ifTrue: [newMid add: aChangeSet]						ifFalse: [newTail add: aChangeSet]]].	ChangeSet allChangeSets: newHead , newMid , newTail.	Smalltalk isMorphic		ifTrue: [SystemWindow wakeUpTopWindowUponStartup]! !!ObjectsTool methodsFor: 'categories' stamp: 'edc 12/4/2007 15:34'!showCategory: aCategoryName fromButton: aButton 	"Project items from the given category into my lower pane"	| quads |	"self partsBin removeAllMorphs. IMHO is redundant, "		Cursor wait		showWhile: [quads := OrderedCollection new.			Morph withAllSubclasses				do: [:aClass | aClass theNonMetaClass						addPartsDescriptorQuadsTo: quads						if: [:aDescription | aDescription translatedCategories includes: aCategoryName]].			quads := quads						asSortedCollection: [:q1 :q2 | q1 third <= q2 third].			self installQuads: quads fromButton: aButton]! !!PasteUpMorph methodsFor: 'world menu' stamp: 'edc 4/21/2005 09:55'!collapseAll	"Collapse all windows"	self collapseAllWindows.	self collapseNonWindows! !!PasteUpMorph methodsFor: 'world menu' stamp: 'edc 4/21/2005 09:43'!collapseAllWindows	"World  collapseAllWindows  "| tl |tl _ (30@40) asPoint. self submorphsDo:  [:each | (each isKindOf: SystemWindow  )ifTrue: [ each collapse. each topLeft: tl .tl _ tl +( 0@30) asPoint]]! !!StringHolder methodsFor: '*Tools' stamp: 'edc 7/31/2007 07:30'!copySelector	"Copy the selected selector to the clipboard"	|  class selector  |	class := self selectedClassOrMetaClass printString.	selector := self selectedMessageName printString.	(selector := self selectedMessageName) ifNotNil:		[Clipboard clipboardText: class, ' ', selector asString]! !!StringHolder methodsFor: 'message list menu' stamp: 'edc 7/16/2005 10:54'!offerDurableMenuFrom: menuRetriever shifted: aBoolean	"Pop up (morphic only) a menu whose target is the receiver and whose contents are provided by sending the menuRetriever to the receiver.  The menuRetriever takes two arguments: a menu, and a boolean representing the shift state; put a stay-up item at the top of the menu."	| aMenu |	aMenu _ MenuMorph new defaultTarget: self.	aMenu addStayUpItem.		self perform: menuRetriever with: aMenu with: aBoolean.		aMenu popUpInWorld! !!CodeHolder methodsFor: 'controls' stamp: 'edc 4/6/2005 10:53'!addOptionalButtonsTo: window at: fractions plus: verticalOffset	"If the receiver wishes it, add a button pane to the window, and answer the verticalOffset plus the height added"	| delta buttons divider width |width _ 10.	self wantsOptionalButtons ifFalse: [^verticalOffset].	delta _ self defaultButtonPaneHeight.	buttons _ self optionalButtonRow 		color: (Display depth <= 8 ifTrue: [Color transparent] ifFalse: [Color gray alpha: 0.2]);		borderWidth: 0.	Preferences alternativeWindowLook ifTrue:[		buttons color: Color transparent.		buttons submorphsDo:[:m| m borderWidth: 2; borderColor: #raised.width _ width + m width].	].	divider _ BorderedSubpaneDividerMorph forBottomEdge.	Preferences alternativeWindowLook ifTrue:[		divider extent: 4@4; color: Color transparent; borderColor: #raised; borderWidth: 2.	].window width: width.	window 		addMorph: buttons		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@verticalOffset corner: 0@(verticalOffset + delta - 1))).	window 		addMorph: divider		fullFrame: (LayoutFrame 				fractions: fractions 				offsets: (0@(verticalOffset + delta - 1) corner: 0@(verticalOffset + delta))).	^ verticalOffset + delta! !!CodeHolder methodsFor: 'controls' stamp: 'edc 4/6/2005 10:48'!optionalButtonPairs	"Answer a tuple (formerly pairs) defining buttons, in the format:			button label			selector to send			help message"	| aList |	aList _ #(	('browse'			browseMethodFull			'view this method in a browser')	('local senders' browseLocalSendersOfMessages 'browse local senders of...')	('senders' 			browseSendersOfMessages	'browse senders of...')	('implementors'		browseMessages				'browse implementors of...')	('versions'			browseVersions				'browse versions')), 	(Preferences decorateBrowserButtons		ifTrue:			[{#('inheritance'		methodHierarchy 'browse method inheritancegreen: sends to supertan: has override(s)mauve: both of the abovepink: is an override but doesn''t call superpinkish tan: has override(s), also is an override but doesn''t call super' )}]		ifFalse:			[{#('inheritance'		methodHierarchy			'browse method inheritance')}]),	#(	('hierarchy'		classHierarchy				'browse class hierarchy')	('inst vars'			browseInstVarRefs			'inst var refs...')	('class vars'			browseClassVarRefs			'class var refs...')).	^ aList! !!Browser methodsFor: 'message functions' stamp: 'ar 1/3/2010 16:25'!messageListMenu: aMenu shifted: shifted 	"Answer the message-list menu"	(self menuHook: aMenu named: #messageListMenu shifted: shifted) ifTrue:[^aMenu].	shifted ifTrue: [^ self shiftedMessageListMenu: aMenu].	aMenu addList: #(			('what to show...'			offerWhatToShowMenu)			('toggle break on entry'		toggleBreakOnEntry)			-			('browse full (b)' 			browseMethodFull)			('browse hierarchy (h)'			classHierarchy)			('browse method (O)'			openSingleMessageBrowser)			('browse protocol (p)'			browseFullProtocol)			-			('fileOut'				fileOutMessage)			('printOut'				printOutMessage)			-			('senders of... (n)'			browseSendersOfMessages)			('implementors of... (m)'		browseMessages)			('inheritance (i)'			methodHierarchy)			('versions (v)'				browseVersions)			-			('inst var refs...'			browseInstVarRefs)			('inst var defs...'			browseInstVarDefs)			('class var refs...'			browseClassVarRefs)			('class variables'			browseClassVariables)			('class refs (N)'			browseClassRefs)			-			('remove method (x)'			removeMessage)			-			('more...'				shiftedYellowButtonActivity)).	^ aMenu! !!ChangeSorter methodsFor: 'changeSet menu' stamp: 'edc 9/23/2005 09:11'!changeSetMenu: aMenu shifted: isShifted 	"Set up aMenu to hold commands for the change-set-list pane. 	This could be for a single or double changeSorter"	isShifted		ifTrue: [^ self shiftedChangeSetMenu: aMenu].	Smalltalk isMorphic		ifTrue: [aMenu title: 'Change Set'.			aMenu addStayUpItemSpecial]		ifFalse: [aMenu title: 'Change Set:' , myChangeSet name].	aMenu add: 'make changes go to me (m)' action: #newCurrent.	aMenu addLine.	aMenu add: 'new change set... (n)' action: #newSet.	aMenu add: 'find...(f)' action: #findCngSet.	aMenu add: 'show category... (s)' action: #chooseChangeSetCategory.	aMenu balloonTextForLastItem: 'Lets you choose which change sets should be listed in this change sorter'.	aMenu add: 'select change set...' action: #chooseCngSet.	aMenu addLine.	aMenu add: 'rename change set (r)' action: #rename.	aMenu add: 'file out (o)' action: #fileOut.	aMenu add: 'file out Compressed' action: #fileOutCompressed.	aMenu add: 'browse methods (b)' action: #browseChangeSet.	aMenu add: 'browse change set (B)' action: #openChangeSetBrowser.	aMenu addLine.	parent		ifNotNil: [aMenu add: 'copy all to other side (c)' action: #copyAllToOther.			aMenu add: 'submerge into other side' action: #submergeIntoOtherSide.			aMenu add: 'subtract other side (-)' action: #subtractOtherSide.			aMenu addLine].	myChangeSet hasPreamble		ifTrue: [aMenu add: 'edit preamble (p)' action: #addPreamble.			aMenu add: 'remove preamble' action: #removePreamble]		ifFalse: [aMenu add: 'add preamble (p)' action: #addPreamble].	myChangeSet hasPostscript		ifTrue: [aMenu add: 'edit postscript...' action: #editPostscript.			aMenu add: 'remove postscript' action: #removePostscript]		ifFalse: [aMenu add: 'add postscript...' action: #editPostscript].	aMenu addLine.	aMenu add: 'category functions...' action: #offerCategorySubmenu.	aMenu balloonTextForLastItem: 'Various commands relating to change-set-categories'.	aMenu addLine.	aMenu add: 'destroy change set (x)' action: #remove.	aMenu addLine.	aMenu add: 'more...' action: #offerShiftedChangeSetMenu.	^ aMenu! !!ChangeSorter methodsFor: 'changeSet menu' stamp: 'edc 11/18/2007 08:26'!fileOut	"File out the current change set."	myChangeSet fileOut.	parent modelWakeUp.	"notice object conversion methods created"! !!ChangeSorter methodsFor: 'changeSet menu' stamp: 'edc 7/16/2005 11:02'!fileOutCompressed	"File out the current change set."	myChangeSet fileOutCompressed.	parent modelWakeUp.	"notice object conversion methods created"! !!ChangeSorter methodsFor: 'message list' stamp: 'dtl 2/27/2010 08:10'!messageMenu: aMenu shifted: shifted	"Fill aMenu with items appropriate for the message list; could be for a single or double changeSorter"	shifted ifTrue: [^ self shiftedMessageMenu: aMenu].	aMenu title: 'message list'.	aMenu addStayUpItemSpecial.	parent ifNotNil:		[aMenu addList: #(			('copy method to other side'			copyMethodToOther)			('move method to other side'			moveMethodToOther))].	aMenu addList: #(			('delete method from changeSet (d)'	forget)			-			('remove method from system (x)'	removeMessage)				-			('browse full (b)'					browseMethodFull)			('browse hierarchy (h)'				spawnHierarchy)			('browse method (O)'				openSingleMessageBrowser)			('browse protocol (p)'				browseFullProtocol)			-			('fileOut'							fileOutMessage)			('printOut'							printOutMessage)			-			('senders of... (n)'					browseSendersOfMessages)			('implementors of... (m)'				browseMessages)			('inheritance (i)'					methodHierarchy)			('versions (v)'						browseVersions)			-			('more...'							shiftedYellowButtonActivity)).	^ aMenu! !!Inspector methodsFor: 'accessing' stamp: 'edc 2/16/2007 08:27'!baseFieldList	"Answer an Array consisting of 'self' 	and the instance variable names of the inspected object."	^ (Array with: 'self' with: 'all inst vars')		, object class allInstVarNames asSortedCollection! !!Inspector methodsFor: 'menu commands' stamp: 'edc 11/18/2007 07:26'!defsOfSelection	"Open a browser on all defining references to the selected instance variable, if that's what currently selected. "	| aClass sel |	self selectionUnmodifiable ifTrue: [^ self changed: #flash].	(aClass := self object class) isVariable ifTrue: [^ self changed: #flash].	sel := aClass allInstVarNames asSortedCollection at: self selectionIndex - 2.	self systemNavigation  browseAllStoresInto: sel from: aClass! !!Inspector methodsFor: 'selecting' stamp: 'edc 2/16/2007 10:05'!selection	"The receiver has a list of variables of its inspected object.  	One of these is selected. Answer the value of the selected  	variable."	| basicIndex varName |	selectionIndex = 0		ifTrue: [^ ''].	selectionIndex = 1		ifTrue: [^ object].	selectionIndex = 2		ifTrue: [^ object longPrintString].	selectionIndex - 2 <= object class instSize		ifTrue: [varName := object class allInstVarNames asSortedCollection at: selectionIndex - 2 .			^ object instVarNamed: varName].	basicIndex := selectionIndex - 2 - object class instSize.	(object basicSize <= (self i1 + self i2)			or: [basicIndex <= self i1])		ifTrue: [^ object basicAt: basicIndex]		ifFalse: [^ object basicAt: object basicSize - (self i1 + self i2) + basicIndex]! !!TheWorldMenu methodsFor: 'windows & flaps menu' stamp: 'edc 4/21/2005 09:57'!windowsMenu        "Build the windows menu for the world."        ^ self fillIn: (self menu: 'windows') from: {                  { 'find window' . { #myWorld . #findWindow: }. 'Presents a list of all windows; if you choose one from the list, it becomes the active window.'}.                { 'find changed browsers...' . { #myWorld . #findDirtyBrowsers: }. 'Presents a list of browsers that have unsubmitted changes; if you choose one from the list, it becomes the active window.'}.                { 'find changed windows...' . { #myWorld . #findDirtyWindows: }. 'Presents a list of all windows that have unsubmitted changes; if you choose one from the list, it becomes the active window.'}.			nil.                { 'find a transcript (t)' . { #myWorld . #findATranscript: }. 'Brings an open Transcript to the front, creating one if necessary, and makes it the active window'}.               { 'find a fileList (L)' . { #myWorld . #findAFileList: }. 'Brings an open fileList  to the front, creating one if necessary, and makes it the active window'}.               { 'find a change sorter (C)' . { #myWorld . #findAChangeSorter: }. 'Brings an open change sorter to the front, creating one if necessary, and makes it the active window'}.			{ 'find message names (W)' . { #myWorld . #findAMessageNamesWindow: }. 'Brings an open MessageNames window to the front, creating one if necessary, and makes it the active window'}.			 nil.                { #staggerPolicyString . { self . #toggleWindowPolicy }. 'stagger: new windows positioned so you can see a portion of each one.                tile: new windows positioned so that they do not overlap others, if possible.'}.                nil.                { 'collapse all windows' . { #myWorld . #collapseAllWindows }. 'Reduce all open windows to collapsed forms that only show titles.'}.                { 'expand all windows' . { #myWorld . #expandAll }. 'Expand all collapsed windows back to their expanded forms.'}.                { 'close top window (w)' . { SystemWindow . #closeTopWindow }. 'Close the topmost window if possible.'}.                { 'send top window to back (\)' . { SystemWindow . #sendTopWindowToBack  }. 'Make the topmost window become the backmost one, and activate the window just beneath it.'}.			 { 'move windows onscreen' . { #myWorld . #bringWindowsFullOnscreen }. 'Make all windows fully visible on the screen'}.                nil.                { 'delete unchanged windows' . { #myWorld . #closeUnchangedWindows }. 'Deletes all windows that do not have unsaved text edits.'}.                { 'delete non-windows' . { #myWorld . #deleteNonWindows }. 'Deletes all non-window morphs lying on the world.'}.                { 'delete both of the above' . { self . #cleanUpWorld }. 'deletes all unchanged windows and also all non-window morphs lying on the world, other than flaps.'}.        }! !