"Change Set:		7893Morphic-ar.198Morphic-ar.198:Removes some more MVC dependencies from FileList and replaces them with proper Toolbuilder variants.Morphic-ml.196:Using minWidth and minHeight instead of minExtent to determine the bounds within a ProportionalSplitterMorph can be dragged.This gives more predictable and expected results than previously. It also results in better behaviour when the system window gets resized to a smaller size.Morphic-ar.197:First pass on a FileList based on ToolBuilder. Slightly different layout; providing the directory view in full height and allowing editing both match pattern as well as directory via input field. Subclasses will be rewhacked later."!MorphicModel subclass: #MorphicModel3	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Models'!StringHolder subclass: #FileList	instanceVariableNames: 'fileName directory volList volListIndex list listIndex pattern sortMode brevityState directoryCache'	classVariableNames: 'FileReaderRegistry RecentDirs'	poolDictionaries: ''	category: 'Morphic-FileList'!!FileList methodsFor: 'initialization' stamp: 'ar 10/4/2009 22:11'!initialize	super initialize.	directoryCache := WeakIdentityKeyDictionary new.! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:33'!buildContentPaneWith: builder	| textSpec |	textSpec := builder pluggableTextSpec new.	textSpec 		model: self;		getText: #contents; 		setText: #put:; 		selection: #contentsSelection; 		menu: #fileContentsMenu:shifted:.	^textSpec! !!FileList class methodsFor: 'instance creation' stamp: 'ar 10/4/2009 22:45'!openEditorOn: aFileStream editString: editString	"Open an editor on the given FileStream."	| fileModel topView builder |	fileModel := FileList new setFileStream: aFileStream.	"closes the stream"	builder := ToolBuilder default.	topView := fileModel buildEditorWith: builder.	^builder open: topView.! !!TheWorldMenu methodsFor: 'commands' stamp: 'ar 10/4/2009 22:18'!openFileList	FileList open.! !!FileList methodsFor: 'private' stamp: 'ar 10/4/2009 20:48'!defaultContents	contents := list == nil		ifTrue: [String new]		ifFalse: [String streamContents:					[:s | s nextPutAll: 'NO FILE SELECTED' translated; cr]].	brevityState := #FileList.	^ contents! !!FileList methodsFor: 'volume list and pattern' stamp: 'ar 10/4/2009 21:08'!volumeListIndex: index	"Select the volume name having the given index."	| delim path |	volListIndex := index.	index = 1 		ifTrue: [self directory: (FileDirectory on: '')]		ifFalse: [delim := directory pathNameDelimiter.				path := String streamContents: [:strm |					2 to: index do: [:i |						strm nextPutAll: (volList at: i) withBlanksTrimmed.						i < index ifTrue: [strm nextPut: delim]]].				self directory: (directory on: path)].	brevityState := #FileList.	self addPath: path.	self changed: #fileList.	self changed: #contents.	self updateButtonRow.! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 20:34'!subDirectoriesOf: aDirectory	^aDirectory directoryNames collect:[:each| aDirectory directoryNamed: each].! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 21:09'!setDirectoryTo: dir	self directory: dir.	brevityState := #FileList.	self changed: #fileList.	self changed: #contents.	self changed: #pathAndPattern.! !!FileList methodsFor: 'initialization' stamp: 'ar 10/4/2009 21:45'!directory: dir	"Set the path of the volume to be displayed."	self okToChange ifFalse: [^ self].	self modelSleep.	directory := dir.	self modelWakeUp.	sortMode == nil ifTrue: [sortMode := #date].	volList := ((Array with: '[]'), directory pathParts)  "Nesting suggestion from RvL"			withIndexCollect: [:each :i | ( String new: i-1 withAll: $ ), each].	volListIndex := volList size.	self changed: #relabel.	self changed: #volumeList.	self pattern: pattern.! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 20:33'!directoryNameOf: aDirectory	^aDirectory localName! !!FileList methodsFor: 'volume list and pattern' stamp: 'ar 10/4/2009 21:46'!pathAndPattern: stringOrText	"Answers both path and pattern"	| base pat aString |	aString := stringOrText asString.	base := aString copyUpToLast: directory pathNameDelimiter.	pat := aString copyAfterLast: directory pathNameDelimiter.	self changed: #pathAndPattern. "avoid asking if it's okToChange"	pattern := pat.	self directory: (FileDirectory on: base).	self changed: #pathAndPattern.	self changed: #selectedPath.! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 21:43'!selectedPath	| top here next |	top := FileDirectory root.	here := directory.	^(Array streamContents:[:s|		s nextPut: here.		[next := here containingDirectory.		top pathName = next pathName] whileFalse:[			s nextPut: next.			here := next.		]]) reversed.! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:40'!getButtonRow	"Answer the dynamic button row to use for the currently selected item."	| builder svc |	builder := ToolBuilder default.	svc := self universalButtonServices.	self fileListIndex = 0 ifFalse:[svc := svc, self dynamicButtonServices].	^svc collect:[:service| service buildWith: builder in: self].! !!FileList class methodsFor: 'instance creation' stamp: 'ar 10/4/2009 20:55'!prototypicalToolWindow	"Answer an example of myself seen in a tool window, for the benefit of parts-launching tools"	^ ToolBuilder build: self new! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 22:42'!buildEditorWith: builder	^super buildWith: builder! !!FileList methodsFor: 'private' stamp: 'ar 10/4/2009 20:44'!entriesMatching: patternString	"Answer a list of directory entries which match the patternString.	The patternString may consist of multiple patterns separated by ';'.	Each pattern can include a '*' or '#' as wildcards - see String>>match:"	| entries patterns |	entries := directory entries reject:[:e| e isDirectory].	patterns := patternString findTokens: ';'.	(patterns anySatisfy: [:each | each = '*'])		ifTrue: [^ entries].	^ entries select: [:entry | patterns anySatisfy: [:each | each match: entry first]]! !!FileList class methodsFor: 'instance creation' stamp: 'ar 10/4/2009 20:55'!open	"Open a view of an instance of me on the default directory."	^ToolBuilder open: self! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 22:12'!hasMoreDirectories: aDirectory	(aDirectory isKindOf: FileDirectory) ifFalse:[^true]. "server directory; don't ask"	^directoryCache at: aDirectory ifAbsentPut:[		[aDirectory directoryNames notEmpty] on: Error do:[:ex| true].	].! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 21:45'!buildWith: builder	"FileList open"	| windowSpec window |	windowSpec := 	self buildWindowWith: builder specs: {		(0@0 corner: 1@0.06) -> [self buildPatternInputWith: builder].		(0.25@0.06 corner: 1@0.15) -> [self buildButtonPaneWith: builder].		(0@0.06 corner: 0.25@1) -> [self buildDirectoryTreeWith: builder].		(0.25@0.15 corner: 1@0.5) -> [self buildFileListWith: builder].		(0.25@0.5 corner: 1@1) -> [self buildContentPaneWith: builder].	}.	window := builder build: windowSpec.	self changed: #selectedPath.	^window! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:40'!buildButtonPaneWith: builder	| panelSpec |	panelSpec := builder pluggablePanelSpec new.	panelSpec 		model: self;		children: #getButtonRow;		layout: #horizontal.	^panelSpec! !!FileList class methodsFor: 'instance creation' stamp: 'ar 10/4/2009 22:10'!newOn: aDirectory	^super new directory: aDirectory! !!FileList methodsFor: 'volume list and pattern' stamp: 'ar 10/4/2009 21:07'!pathAndPattern	"Answers both path and pattern"	^directory fullName, directory slash, pattern! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 21:17'!buildPatternInputWith: builder	| textSpec |	textSpec := builder pluggableInputFieldSpec new.	textSpec 		model: self;		getText: #pathAndPattern; 		setText: #pathAndPattern:.	^textSpec! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:51'!executeService: aService	aService performServiceFor: self.! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:32'!buildFileListWith: builder	| listSpec |	listSpec := builder pluggableListSpec new.	listSpec 		model: self;		list: #fileList; 		getIndex: #fileListIndex; 		setIndex: #fileListIndex:; 		menu: #fileListMenu:; 		keyPress: nil.	^listSpec! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 21:34'!getSelectedPath	self halt.! !!FileList class methodsFor: 'instance creation' stamp: 'ar 10/4/2009 22:10'!new	^self newOn: FileDirectory default! !!FileList methodsFor: 'directory tree' stamp: 'ar 10/4/2009 20:34'!rootDirectoryList	| dirList dir servers |	dir := FileDirectory on: ''.	dirList := dir directoryNames collect:[:each| dir directoryNamed: each]..	dirList isEmpty ifTrue:[dirList := Array with: FileDirectory default].	servers := ServerDirectory serverNames collect: [ :n | ServerDirectory serverNamed: n].	"This is so FileListPlus will work on ancient Squeak versions."	servers := servers select:[:each| each respondsTo: #localName].	^dirList, servers! !!ProportionalSplitterMorph methodsFor: 'as yet unclassified' stamp: 'ml 10/2/2009 18:09'!minimumHeightOf: aCollection	"Answer the minimum height needed to display any of the morphs in aCollection."	^ aCollection inject: 0 into: [ :height :morph |		(morph minHeight + self height) max: height]! !!ProportionalSplitterMorph methodsFor: 'as yet unclassified' stamp: 'ml 10/2/2009 18:08'!minimumWidthOf: aCollection	"Answer the minimum width needed to display any of the morphs in aCollection."	^ aCollection inject: 0 into: [ :width :morph |		(morph minWidth + self width) max: width]! !!FileList methodsFor: 'initialization' stamp: 'ar 10/4/2009 20:49'!updateButtonRow	"Dynamically update the contents of the button row, if any."	self changed: #getButtonRow.! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 20:32'!buildDirectoryTreeWith: builder	| treeSpec |	treeSpec := builder pluggableTreeSpec new.	treeSpec 			model: self;			roots: #rootDirectoryList;			hasChildren: #hasMoreDirectories:;			getChildren: #subDirectoriesOf:;			getSelectedPath: #selectedPath; 			setSelected: #setDirectoryTo:;			label: #directoryNameOf:;			autoDeselect: false.	^treeSpec! !!FileList methodsFor: 'initialization' stamp: 'ar 10/4/2009 22:48'!labelString	^fileName ifNil:['File List'] ifNotNil:[directory fullNameFor: fileName].! !!FileList methodsFor: 'toolbuilder' stamp: 'ar 10/4/2009 22:44'!buildCodePaneWith: builder	| textSpec |	textSpec := builder pluggableTextSpec new.	textSpec 		model: self;		getText: #contents; 		setText: #put:; 		selection: #contentsSelection; 		menu: #fileContentsMenu:shifted:.	^textSpec! !!FileList2 methodsFor: 'initialization' stamp: 'ar 10/4/2009 21:31'!optionalButtonRow	"Answer the button row associated with a file list"	| aRow |	aRow := AlignmentMorph newRow beSticky.	aRow color: Color transparent.	aRow clipSubmorphs: true.	aRow layoutInset: 5@1; cellInset: 6.	self universalButtonServices do:  "just the three sort-by items"			[:service |				aRow addMorphBack: (service buttonToTriggerIn: self).				(service selector  == #sortBySize)					ifTrue:						[aRow addTransparentSpacerOfSize: (4@0)]].	aRow setNameTo: 'buttons'.	aRow setProperty: #buttonRow toValue: true.  "Used for dynamic retrieval later on"	^ aRow! !FileList class removeSelector: #openAsMorph!FileList removeSelector: #optionalButtonView!FileList removeSelector: #isDirectoryList:!FileList removeSelector: #optionalButtonRow!FileList removeSelector: #primitiveCopyFileNamed:to:!FileList class removeSelector: #defaultButtonPaneHeight!FileList removeSelector: #dragTransferTypeForMorph:!FileList removeSelector: #dragPassengerFor:inMorph:!FileList class removeSelector: #openMorphOn:editString:!FileList removeSelector: #openMorphFromFile!FileList removeSelector: #wantsDroppedMorph:event:inMorph:!FileList class removeSelector: #addButtonsAndFileListPanesTo:at:plus:forFileList:!FileList class removeSelector: #addVolumesAndPatternPanesTo:at:plus:forFileList:!FileList removeSelector: #dropDestinationDirectory:event:!FileList removeSelector: #acceptDroppingMorph:event:inMorph:!