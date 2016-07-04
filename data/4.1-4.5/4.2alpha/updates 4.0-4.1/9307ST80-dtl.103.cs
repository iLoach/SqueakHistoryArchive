"Change Set:		9307ST80-dtl.103ST80-dtl.103:Handle #saveContents update from Workspace in PluggableTextView. Removes MVC/Morphic dependency in Workspace.Handle #close update from TranscriptStream in PluggableTextView. Removes MVC/Morphic dependency in Workspace.Add PluggableTextView>>isTextView to support TranscriptStream>>countOpenTranscripts without #isKindOf: tests on MVC and Morphic views.ST80-dtl.92:Add PluggableFileList>>mvcOpenLable:in: to remove explicit MVC dependency in PluggableFileList.ST80-dtl.93:Move PluggableFileListView from package Morphic-FileList to ST80-ViewsMove ModalSystemWindowView from package Morphic-FileList to ST80-Views ST80-dtl.94:Move SelectionMenu, CustomMenu, and EmphasizedMenu from ST80-Menus to Tools-Menus. These classes are not MVC specific.ST80-nice.95:make cr-indentation work in case of LFclean up dead code in inOutdent:delta:Note: currently, shift+cmd+L will outdent even if line with min outdent is zero. his was the old behaviour, but we can change it by uncommenting the '^false'ST80-nice.96:Let outdent work when selection starts on a wrapped line.Do not let indent insert a tab in the middle of a line when selection starts on a wrapped line.ST80-nice.97:Let indent/outdent work in case of LFST80-dtl.98:Remove explicit MVC and Morphic dependencies from SyntaxError. SyntaxError is a model in Debugger (not an exception class).ST80-dtl.99:Move #asParagraph methods to package *ST80-Support.Remove explicit MVC and Morphic dependencies from MailComposition.Change method categories for SyntaxError from *MVC-Models to *MVC-Support.ST80-dtl.100:Move #fullScreenOn and #fullScreenOff implementation from ScreenController to ProjectST80-dtl.101:Fix SelectorBrowser>>classListIndex: to use #changed: #update: to notify MVC views that control should be terminated prior to opening a new browser. Original implemention polled dependents with #isKIndOf: and had explicit dependency on MVC PluggableListView.ST80-dtl.102:Remove VMC/Morphic dependencies from Form, FileList2, and ImportsAdd MVCProject>>formViewClassAdd MVCProject>>openImage:name:saveResource:"!ModalSystemWindowView subclass: #PluggableFileListView	instanceVariableNames: 'acceptButtonView'	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Views'!Controller subclass: #ScreenController	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Controllers'!StandardSystemView subclass: #ModalSystemWindowView	instanceVariableNames: 'modalBorder'	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Views'!!ParagraphEditor methodsFor: 'typing/selecting keys' stamp: 'nice 2/8/2010 10:56'!crWithIndent: characterStream 	"Replace the current text selection with CR followed by as many tabs	as on the current line (+/- bracket count) -- initiated by Shift-Return."	| char s i tabCount |	sensor keyboard.		"flush character"	s := paragraph string.	i := self stopIndex.	tabCount := 0.	[(i := i-1) > 0 and: [(char := s at: i) ~= Character cr and: [char ~= Character lf]]]		whileTrue:  "Count tabs and brackets (but not a leading bracket)"		[(char = Character tab and: [i < s size and: [(s at: i+1) ~= $[ ]]) ifTrue: [tabCount := tabCount + 1].		char = $[ ifTrue: [tabCount := tabCount + 1].		char = $] ifTrue: [tabCount := tabCount - 1]].	characterStream crtab: tabCount.  "Now inject CR with tabCount tabs"	^ false! !!ModalSystemWindowView methodsFor: 'model access' stamp: 'acg 2/9/2000 00:57'!update: aSymbol	aSymbol = #close		ifTrue: [^self controller close].	^super update: aSymbol! !!ModalSystemWindowView methodsFor: 'modal dialog' stamp: 'BG 12/13/2002 11:33'!doModalDialog	| savedArea |	self resizeInitially.	self resizeTo: 		((self windowBox)			align: self windowBox center			with: Display boundingBox aboveCenter).	savedArea := Form fromDisplay: self windowBox.	self displayEmphasized.	self controller startUp.	self release.	savedArea displayOn: Display at: self windowOrigin.! !!PluggableTextView methodsFor: 'updating' stamp: 'dtl 2/10/2010 17:15'!update: aSymbol	"Refer to the comment in View|update:. Do nothing if the given symbol does not match any action. "	aSymbol == #wantToChange ifTrue:			[self canDiscardEdits ifFalse: [self promptForCancel].  ^ self].	aSymbol == #flash ifTrue: [^ controller flash].	aSymbol == getTextSelector ifTrue: [^ self updateDisplayContents].	aSymbol == getSelectionSelector ifTrue: [^ self setSelection: self getSelection].	aSymbol == #clearUserEdits ifTrue: [^ self hasUnacceptedEdits: false].	(aSymbol == #autoSelect and: [getSelectionSelector ~~ nil]) ifTrue:			[ParagraphEditor abandonChangeText.	"no replacement!!"			^ controller setSearch: model autoSelectString;					againOrSame: true].	aSymbol == #appendEntry ifTrue:			[^ controller doOccluded: [controller appendEntry]].	aSymbol == #clearText ifTrue:			[^ controller doOccluded:				[controller changeText: Text new]].	aSymbol == #bs ifTrue:			[^ controller doOccluded:				[controller bsText]].	aSymbol == #codeChangedElsewhere ifTrue:			[^ self hasEditingConflicts: true].	aSymbol == #saveContents ifTrue:			[^self controller saveContentsInFile].	aSymbol == #close ifTrue:			[^self topView controller closeAndUnscheduleNoTerminate]! !!ScreenController methodsFor: 'menu messages' stamp: 'dtl 2/10/2010 10:23'!fullScreenOff	Project current fullScreenOff! !!ModalSystemWindowView methodsFor: 'initialize-release' stamp: 'acg 2/18/2000 20:41'!borderWidth: anObject	modalBorder := false.	^super borderWidth: anObject! !!ParagraphEditor methodsFor: 'menu messages' stamp: 'nice 2/8/2010 10:48'!selectedSymbol	"Return the currently selected symbol, or nil if none.  Spaces, tabs and returns are ignored"	| aString |	self hasCaret ifTrue: [^ nil].	aString := self selection string.	aString isOctetString ifTrue: [aString := aString asOctetString].	aString := self selection string copyWithoutAll: CharacterSet separators.	aString size == 0 ifTrue: [^ nil].	Symbol hasInterned: aString  ifTrue: [:sym | ^ sym].	^ nil! !!PluggableFileListView methodsFor: 'as yet unclassified' stamp: 'acg 2/18/2000 20:52'!label: aString	super label: aString.	self noLabel! !!PluggableTextView methodsFor: 'testing' stamp: 'dtl 2/10/2010 17:28'!isTextView	"True if the reciever is a view on a text model, such as a view on a TranscriptStream"	^true! !!ModalSystemWindowView methodsFor: 'displaying' stamp: 'acg 2/19/2000 00:59'!displayBorder	"Display the receiver's border (using the receiver's borderColor)."	modalBorder ifFalse: [^super displayBorder].	Display		border: self displayBox		widthRectangle: (1@1 corner: 2@2)		rule: Form over		fillColor: Color black.	Display		border: (self displayBox insetBy: (1@1 corner: 2@2))		widthRectangle: (4@4 corner: 3@3)		rule: Form over		fillColor: (Color r: 16rEA g: 16rEA b: 16rEA).! !!ModalSystemWindowView methodsFor: 'displaying' stamp: 'acg 2/18/2000 20:24'!display	super display.	self displayLabelBackground: false.	self displayLabelText.! !!Text methodsFor: '*ST80-Support' stamp: ''!asParagraph	"Answer a Paragraph whose text is the receiver."	^Paragraph withText: self! !!ModalSystemWindowView methodsFor: 'label access' stamp: 'acg 2/9/2000 08:35'!backgroundColor	^Color lightYellow! !!MVCProject methodsFor: 'editors' stamp: 'dtl 2/10/2010 13:19'!formViewClass	"Answer a class suitable for a view on a form or collection of forms"	^ FormInspectView! !!PluggableListView methodsFor: 'updating' stamp: 'dtl 2/10/2010 13:01'!update: aSymbol 	"Refer to the comment in View|update:."	aSymbol == getListSelector ifTrue:		[self list: self getList.		self displayView.		self displaySelectionBox.		^self].	aSymbol == getSelectionSelector ifTrue:		[^ self moveSelectionBox: self getCurrentSelectionIndex].	aSymbol == #startNewBrowser ifTrue:		[(self setSelectionSelectorIs: #classListIndex:) ifTrue: [			"A SelectorBrowser is about to open a new Browser on a class"			self controller controlTerminate]]! !!PluggableFileListView methodsFor: 'as yet unclassified' stamp: 'acg 2/9/2000 09:40'!updateAcceptButton	self model canAccept		ifTrue:			[acceptButtonView				backgroundColor: Color green;				borderWidth: 3;				controller: acceptButtonView defaultController]		ifFalse:			[acceptButtonView				backgroundColor: Color lightYellow;				borderWidth: 1;				controller: NoController new].	acceptButtonView display.! !!ParagraphEditor methodsFor: 'editing keys' stamp: 'nice 2/9/2010 09:32'!inOutdent: characterStream delta: delta	"Add/remove a tab at the front of every line occupied by the selection. Flushes typeahead.  Derived from work by Larry Tesler back in December 1985.  Now triggered by Cmd-L and Cmd-R.  2/29/96 sw"	| realStart realStop lines startLine stopLine start stop adjustStart indentation numLines oldString newString newSize |	sensor keyboard.  "Flush typeahead"	"Operate on entire lines, but remember the real selection for re-highlighting later"	realStart := self startIndex.	realStop := self stopIndex - 1.	"Special case a caret on a line of its own, including weird case at end of paragraph"	(realStart > realStop and:				[realStart < 2 or: [(paragraph string at: realStart - 1) == Character cr or: [(paragraph string at: realStart - 1) == Character lf]]])		ifTrue:			[delta < 0				ifTrue:					[view flash]				ifFalse:					[self replaceSelectionWith: Character tab asSymbol asText.					self selectAt: realStart + 1].			^true].	lines := paragraph lines.	startLine := paragraph lineIndexOfCharacterIndex: realStart.	"start on a real line, not a wrapped line"	[startLine = 1 or: [CharacterSet crlf includes: (paragraph string at: (lines at: startLine-1) last)]] whileFalse: [startLine := startLine - 1].	stopLine := paragraph lineIndexOfCharacterIndex: (realStart max: realStop).	start := (lines at: startLine) first.	stop := (lines at: stopLine) last.		"Pin the start of highlighting unless the selection starts a line"	adjustStart := realStart > start.	"Find the indentation of the least-indented non-blank line; never outdent more"	indentation := (startLine to: stopLine) inject: 1000 into:		[:m :l |		m min: (paragraph indentationOfLineIndex: l ifBlank: [:tabs | 1000])].	indentation + delta <= 0 ifTrue: ["^false"].	numLines := stopLine + 1 - startLine.	oldString := paragraph string copyFrom: start to: stop.	newString := oldString species new: oldString size + ((numLines * delta) max: 0).		"Do the actual work"	newSize := 0.	delta > 0		ifTrue: [| tabs |			tabs := oldString species new: delta withAll: Character tab.			oldString lineIndicesDo: [:startL :endWithoutDelimiters :endL |				startL < endWithoutDelimiters ifTrue: [newString replaceFrom: 1 + newSize to: (newSize := newSize + delta) with: tabs startingAt: 1].				newString replaceFrom: 1 + newSize to: (newSize := 1 + newSize + endL - startL) with: oldString startingAt: startL]]		ifFalse: [| tab |			tab := Character tab.			oldString lineIndicesDo: [:startL :endWithoutDelimiters :endL |				| i |				i := 0.				[i + delta < 0 and: [ i + startL <= endWithoutDelimiters and: [(oldString at: i + startL) == tab]]] whileTrue: [i := i + 1].				newString replaceFrom: 1 + newSize to: (newSize := 1 + newSize + endL - (i + startL)) with: oldString startingAt: i + startL]].	newSize < newString size ifTrue: [newString := newString copyFrom: 1 to: newSize].		"Adjust the range that will be highlighted later"	adjustStart ifTrue: [realStart := (realStart + delta) max: start].	realStop := realStop + newSize - oldString size.	"Replace selection"	self selectInvisiblyFrom: start to: stop.	self replaceSelectionWith: newString asText.	self selectFrom: realStart to: realStop. 	"highlight only the original range"	^ true! !!PluggableFileListView methodsFor: 'as yet unclassified' stamp: 'acg 2/9/2000 08:57'!acceptButtonView: aView	^acceptButtonView := aView! !!ModalSystemWindowView methodsFor: 'initialize-release' stamp: 'acg 2/19/2000 00:50'!initialize 	"Refer to the comment in View|initialize."	super initialize.	self borderWidth: 5.	self noLabel.	modalBorder := true.! !!ModalSystemWindowView methodsFor: 'displaying' stamp: 'acg 2/9/2000 07:21'!displayLabelBoxes	"Modal dialogs don't have closeBox or growBox."! !!SyntaxError class methodsFor: '*ST80-Support' stamp: 'sd 11/20/2005 21:28'!buildMVCViewOn: aSyntaxError	"Answer an MVC view on the given SyntaxError."	| topView aListView aCodeView |	topView := StandardSystemView new		model: aSyntaxError;		label: 'Syntax Error';		minimumSize: 380@220.	aListView := PluggableListView on: aSyntaxError		list: #list		selected: #listIndex		changeSelected: nil		menu: #listMenu:.	aListView window: (0@0 extent: 380@20).	topView addSubView: aListView.	aCodeView := PluggableTextView on: aSyntaxError		text: #contents		accept: #contents:notifying:		readSelection: #contentsSelection		menu: #codePaneMenu:shifted:.	aCodeView window: (0@0 extent: 380@200).	topView addSubView: aCodeView below: aListView.	^ topView! !!PluggableFileList methodsFor: '*ST80-Pluggable Views' stamp: 'dtl 2/2/2010 21:13'!mvcOpenLabel: ignored in: aWorld	"Open a view of an instance of me."	"PluggableFileList new open"	| topView volListView templateView fileListView fileStringView leftButtonView middleButtonView rightButtonView |		self directory: directory.	topView := (PluggableFileListView new)		model: self.	volListView := PluggableListView on: self		list: #volumeList		selected: #volumeListIndex		changeSelected: #volumeListIndex:		menu: #volumeMenu:.	volListView autoDeselect: false.	volListView window: (0@0 extent: 80@45).	topView addSubView: volListView.	templateView := PluggableTextView on: self		text: #pattern		accept: #pattern:.	templateView askBeforeDiscardingEdits: false.	templateView window: (0@0 extent: 80@15).	topView addSubView: templateView below: volListView.	fileListView := PluggableListView on: self		list: #fileList		selected: #fileListIndex		changeSelected: #fileListIndex:		menu: #fileListMenu:.	fileListView window: (0@0 extent: 120@60).	topView addSubView: fileListView toRightOf: volListView.	fileListView controller terminateDuringSelect: true.  "Pane to left may change under scrollbar"	"fileStringView := PluggableTextView on: self		text: #fileString		accept: #fileString:.	fileStringView askBeforeDiscardingEdits: false.	fileStringView window: (0@0 extent: 200@15).	topView addSubView: fileStringView below: templateView."	fileStringView := templateView.	leftButtonView := PluggableButtonView 		on: self		getState: nil		action: #leftButtonPressed.	leftButtonView		label: 'Cancel';		backgroundColor: Color red;		borderWidth: 3;		window: (0@0 extent: 50@15).	middleButtonView := PluggableButtonView		on: self		getState: nil		action: nil.	middleButtonView		label: prompt;		window: (0@0 extent: 100@15);		borderWidth: 1;		controller: NoController new.	rightButtonView := PluggableButtonView		on: self		getState: nil		action: #rightButtonPressed.	rightButtonView		label: 'Accept';		backgroundColor: (self canAccept ifTrue: [Color green] ifFalse: [Color lightYellow]);		borderWidth: (self canAccept ifTrue: [3] ifFalse: [1]);		window: (0@0 extent: 50@15).	self canAccept ifFalse: [rightButtonView controller: NoController new].	topView acceptButtonView: rightButtonView.	topView		addSubView: leftButtonView below: fileStringView;		addSubView: middleButtonView toRightOf: leftButtonView;		addSubView: rightButtonView toRightOf: middleButtonView.	self changed: #getSelectionSel.	topView doModalDialog.		^self result! !!String methodsFor: '*ST80-Support' stamp: 'yo 11/3/2004 19:24'!asParagraph	"Answer a Paragraph whose text string is the receiver."	^Paragraph withText: self asText! !!DisplayText methodsFor: '*ST80-Support' stamp: 'tk 10/21/97 12:28'!asParagraph	"Answer a Paragraph whose text and style are identical to that of the 	receiver."	| para |	para := Paragraph withText: text style: textStyle.	para foregroundColor: foreColor backgroundColor: backColor.	backColor isTransparent ifTrue: [para rule: Form paint].	^ para! !!MVCProject methodsFor: 'editors' stamp: 'dtl 2/10/2010 15:27'!openImage: aForm name: fullName saveResource: aBoolean	"Open a view on an image. Do not save project resource in an MVC project."	FormView open: aForm named: fullName! !!ScreenController methodsFor: 'menu messages' stamp: 'dtl 2/10/2010 10:24'!fullScreenOn	Project current fullScreenOn! !!PluggableFileListView methodsFor: 'as yet unclassified' stamp: 'acg 2/9/2000 08:55'!update: aSymbol	(aSymbol = #volumeListIndex or: [aSymbol = #fileListIndex])		ifTrue: [self updateAcceptButton].	^super update: aSymbol! !!MailComposition methodsFor: '*ST80-Support' stamp: 'dtl 2/9/2010 22:34'!mvcOpen	| textView sendButton  |	mvcWindow := StandardSystemView new		label: 'Mister Postman';		minimumSize: 400@250;		model: self.	textView := PluggableTextView		on: self		text: #messageText		accept: #messageText:.	textEditor := textView controller.	sendButton := PluggableButtonView 		on: self		getState: nil		action: #submit.	sendButton label: 'Send'.	sendButton borderWidth: 1.	sendButton window: (1@1 extent: 398@38).	mvcWindow addSubView: sendButton.	textView window: (0@40 corner: 400@250).	mvcWindow addSubView: textView below: sendButton.	mvcWindow controller open.		! !!ModalSystemWindowView methodsFor: 'controller access' stamp: 'dtl 1/24/2010 17:56'!defaultControllerClass	^Smalltalk at: #ModalController! !!SyntaxError class methodsFor: '*ST80-Support' stamp: 'dtl 2/8/2010 23:01'!mvcOpen: aSyntaxError	"Answer a standard system view whose model is an instance of me."	| topView |	topView := self buildMVCViewOn: aSyntaxError.	topView controller openNoTerminateDisplayAt: Display extent // 2.	Cursor normal show.	Processor activeProcess suspend! !CustomMenu removeSelector: #arguments!CustomMenu removeSelector: #initialize!EmphasizedMenu removeSelector: #setEmphasis!CustomMenu removeSelector: #title:!SelectionMenu class removeSelector: #labels:selections:!CustomMenu removeSelector: #add:action:!CustomMenu removeSelector: #build!ScreenController class removeSelector: #lastScreenModeSelected!SelectionMenu class removeSelector: #selections:lines:!EmphasizedMenu removeSelector: #emphases:!SelectionMenu class removeSelector: #labelList:lines:!CustomMenu removeSelector: #invokeOn:orSendTo:!CustomMenu removeSelector: #labels:lines:selections:!Smalltalk removeClassNamed: #EmphasizedMenu!EmphasizedMenu class removeSelector: #example3!EmphasizedMenu removeSelector: #startUpWithCaption:!EmphasizedMenu removeSelector: #onlyBoldItem:!SelectionMenu removeSelector: #invokeOn:orSendTo:!CustomMenu removeSelector: #add:target:selector:argument:!CustomMenu removeSelector: #addLine!CustomMenu removeSelector: #addStayUpItem!CustomMenu removeSelector: #targets!EmphasizedMenu class removeSelector: #selections:emphases:!CustomMenu removeSelector: #balloonTextForLastItem:!CustomMenu removeSelector: #addServices2:for:extraLines:!CustomMenu removeSelector: #labels:font:lines:!CustomMenu removeSelector: #addList:!CustomMenu removeSelector: #addServices:for:extraLines:!SelectionMenu class removeSelector: #labelList:!SelectionMenu class removeSelector: #fromArray:!CustomMenu removeSelector: #invokeOn:defaultSelection:!SelectionMenu class removeSelector: #labels:lines:!SelectionMenu class removeSelector: #labelList:selections:!EmphasizedMenu class removeSelector: #example1!Smalltalk removeClassNamed: #CustomMenu!CustomMenu removeSelector: #addTranslatedList:!CustomMenu removeSelector: #add:subMenu:target:selector:argumentList:!SelectionMenu removeSelector: #startUpWithCaption:at:allowKeyboard:!CustomMenu removeSelector: #startUp:!SelectionMenu removeSelector: #selections:!CustomMenu removeSelector: #addService:for:!CustomMenu removeSelector: #invokeOn:!SelectionMenu class removeSelector: #labelList:lines:selections:!Smalltalk removeClassNamed: #SelectionMenu!CustomMenu class removeSelector: #example!SelectionMenu class removeSelector: #labels:lines:selections:!SelectionMenu removeSelector: #selections!EmphasizedMenu class removeSelector: #selectionAndEmphasisPairs:!CustomMenu removeSelector: #startUpWithCaption:!SelectionMenu removeSelector: #invokeOn:!CustomMenu removeSelector: #preSelect:!CustomMenu removeSelector: #startUp!CustomMenu removeSelector: #startUp:withCaption:!SelectionMenu class removeSelector: #selections:!CustomMenu removeSelector: #add:target:selector:argumentList:!EmphasizedMenu class removeSelector: #example2!ParagraphEditor removeSelector: #indent:fromStream:toStream:!