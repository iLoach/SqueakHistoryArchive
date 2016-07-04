"Change Set:		9391Morphic-edc.347Morphic-edc.347:Enhanced loadProject, now when you click in the dockingbar you have nicer interface.Also., as in my case you have populated disk, the last directory is rememberedMorphic-cmm.343:- Tools enhancement:  before opening a new window on a Model, first check whether an existing window on the desktop can fulfill the role and, if so, bring it to the top.Morphic-cmm.344:- More fixes of window topping feature.Morphic-dtl.345:Implement MorphicProject>>textWindows to remove MVC/Morphic dependencies from  Utilities class> storeTextWindowContentsToFileNamed:Morphic-kb.346: - Added a preference to control the window reusing behaviour. It's disabled by default."!MorphicModel subclass: #SystemWindow	instanceVariableNames: 'labelString stripes label closeBox collapseBox activeOnlyOnTop paneMorphs paneRects collapsedFrame fullFrame isCollapsed menuBox mustNotClose labelWidgetAllowance updatablePanes allowReframeHandles labelArea expandBox'	classVariableNames: 'CloseBoxImage CollapseBoxImage ExpandBoxImage MenuBoxImage ReuseWindows TopWindow'	poolDictionaries: ''	category: 'Morphic-Windows'!!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:29'!openInWorld: aWorld extent: extent	"This msg and its callees result in the window being activeOnlyOnTop"	^ self anyOpenWindowLikeMe		ifEmpty:			[ self 				position: (RealEstateAgent initialFrameFor: self world: aWorld) topLeft ;				extent: extent.			self openAsIsIn: aWorld ]		ifNotEmptyDo:			[ : windows | 			windows anyOne				expand ;				activate ; 				postAcceptBrowseFor: self ]! !!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:07'!postAcceptBrowseFor: anotherSystemWindow	"If I am taking over browsing for anotherSystemWindow, sucblasses may override to, for example, position me to the object to be focused on."	self model postAcceptBrowseFor: anotherSystemWindow model! !!SystemWindow class methodsFor: 'preferences' stamp: 'kb 2/15/2010 11:10'!reuseWindows	<preference: 'Reuse Windows'		category: 'browsing'		description: 'When enabled, before opening a new window check if there is any open window like it, and if there is, reuse it.'		type: #Boolean>	^ReuseWindows ifNil: [ false ]! !!SystemWindow methodsFor: 'open/close' stamp: 'kb 2/15/2010 11:11'!anyOpenWindowLikeMe		self class reuseWindows ifFalse: [ ^Array empty ].	^ SystemWindow		windowsIn: World 		satisfying: 			[ : each |			each model class = self model class				and: [ (each model respondsTo: #representsSameBrowseeAs:) 				and: [ each model representsSameBrowseeAs: self model ] ] ]! !!Model methodsFor: '*morphic' stamp: 'cmm 2/13/2010 20:34'!postAcceptBrowseFor: anotherModel 	"If I am taking over browsing for anotherModel, sucblasses may override to, for example, position me to the object to be focused on."! !!Lexicon methodsFor: '*morphic' stamp: 'cmm 2/23/2002 16:03'!targetClass	^targetClass! !!Model methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:54'!representsSameBrowseeAs: anotherModel	"Answer true if my browser can browse what anotherModel wants to browse."	^ false! !!MorphicProject methodsFor: 'utilities' stamp: 'dtl 2/14/2010 20:49'!textWindows	"Answer a dictionary of all system windows for text display keyed by window title.	Generate new window titles as required to ensure unique keys in the dictionary."	| aDict windows title |	aDict := Dictionary new.	windows := World submorphs select: [:m | m isSystemWindow].	windows do:		[:w | | assoc |		assoc := w titleAndPaneText.		assoc ifNotNil:			[w holdsTranscript ifFalse:				[title := assoc key.				(aDict includesKey: title) ifTrue: [ | newKey | "Ensure unique keys in aDict"					(1 to: 100) detect: [:e |							newKey := title, '-', e asString.							(aDict includesKey: newKey) not].					title := newKey.					assoc := newKey -> assoc value].				aDict add: assoc]]].	^ aDict! !!Inspector methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:51'!representsSameBrowseeAs: anotherInspector	^ self object == anotherInspector object! !!HierarchyBrowser methodsFor: '*morphic' stamp: 'cmm 2/14/2010 20:13'!postAcceptBrowseFor: aHierarchyBrowser 	self		selectClass: aHierarchyBrowser selectedClass ;		selectedMessageName: aHierarchyBrowser selectedMessageName! !!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:09'!openInWorld: aWorld	"This msg and its callees result in the window being activeOnlyOnTop"	self anyOpenWindowLikeMe		ifEmpty: 			[ self 				bounds: (RealEstateAgent initialFrameFor: self world: aWorld) ;				openAsIsIn: aWorld ]		ifNotEmptyDo:			[ : windows | 			windows anyOne				expand ;				activate ; 				postAcceptBrowseFor: self ]! !!HierarchyBrowser methodsFor: '*morphic' stamp: 'cmm 2/14/2010 20:06'!representsSameBrowseeAs: anotherModel	^ self hasUnacceptedEdits not		and: [ classList size = anotherModel classList size		and: [ classList includesAllOf: anotherModel classList ] ]! !!TheWorldMenu methodsFor: 'commands' stamp: 'edc 2/17/2010 10:39'!loadProject	| stdFileMenuResult |	"Put up a Menu and let the user choose a '.project' file to load.  Create a thumbnail and jump into the project."	Project canWeLoadAProjectNow ifFalse: [^ self].	stdFileMenuResult := ((StandardFileMenu new) pattern: '*.pr'; 		oldFileFrom: FileList2 modalFolderSelector) 			startUpWithCaption: 'Select a File:' translated.	stdFileMenuResult ifNil: [^ nil].	ProjectLoading 		openFromDirectory: stdFileMenuResult directory 		andFileName: stdFileMenuResult name! !!Lexicon methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:51'!representsSameBrowseeAs: anotherModel	^self hasUnacceptedEdits not		and: [ anotherModel targetClass = self targetClass ]! !!SystemWindow class methodsFor: 'preferences' stamp: 'kb 2/15/2010 11:11'!reuseWindows: aBoolean	ReuseWindows := aBoolean! !!ObjectExplorer methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:55'!representsSameBrowseeAs: anotherObjectExplorer	^ self rootObject == anotherObjectExplorer rootObject! !!PreferenceBrowser methodsFor: '*morphic' stamp: 'cmm 2/13/2010 20:08'!representsSameBrowseeAs: anotherModel	"If an existing Preference browser is on-screen, use it."	^ self class = anotherModel class! !