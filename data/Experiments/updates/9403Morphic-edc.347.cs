'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!MorphicModel subclass: #SystemWindow	instanceVariableNames: 'labelString stripes label closeBox collapseBox activeOnlyOnTop paneMorphs paneRects collapsedFrame fullFrame isCollapsed menuBox mustNotClose labelWidgetAllowance updatablePanes allowReframeHandles labelArea expandBox '	classVariableNames: 'CloseBoxImage CollapseBoxImage ExpandBoxImage MenuBoxImage TopWindow ReuseWindows '	poolDictionaries: ''	category: 'Morphic-Windows'!!SystemWindow commentStamp: '<historical>' prior: 0!SystemWindow is the Morphic equivalent of StandardSystemView -- a labelled container for rectangular views, with iconic facilities for close, collapse/expand, and resizing.The attribute onlyActiveOnTop, if set to true (and any call to activate will set this), determines that only the top member of a collection of such windows on the screen shall be active.  To be not active means that a mouse click in any region will only result in bringing the window to the top and then making it active.!!Model methodsFor: '*morphic' stamp: 'cmm 2/13/2010 20:34'!postAcceptBrowseFor: anotherModel 	"If I am taking over browsing for anotherModel, sucblasses may override to, for example, position me to the object to be focused on."! !!Model methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:54'!representsSameBrowseeAs: anotherModel	"Answer true if my browser can browse what anotherModel wants to browse."	^ false! !!HierarchyBrowser methodsFor: '*morphic' stamp: 'cmm 2/14/2010 20:13'!postAcceptBrowseFor: aHierarchyBrowser 	self		selectClass: aHierarchyBrowser selectedClass ;		selectedMessageName: aHierarchyBrowser selectedMessageName! !!HierarchyBrowser methodsFor: '*morphic' stamp: 'cmm 2/14/2010 20:06'!representsSameBrowseeAs: anotherModel	^ self hasUnacceptedEdits not		and: [ classList size = anotherModel classList size		and: [ classList includesAllOf: anotherModel classList ] ]! !!Inspector methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:51'!representsSameBrowseeAs: anotherInspector	^ self object == anotherInspector object! !!MorphicProject methodsFor: 'utilities' stamp: 'dtl 2/14/2010 20:49'!textWindows	"Answer a dictionary of all system windows for text display keyed by window title.	Generate new window titles as required to ensure unique keys in the dictionary."	| aDict windows title |	aDict := Dictionary new.	windows := World submorphs select: [:m | m isSystemWindow].	windows do:		[:w | | assoc |		assoc := w titleAndPaneText.		assoc ifNotNil:			[w holdsTranscript ifFalse:				[title := assoc key.				(aDict includesKey: title) ifTrue: [ | newKey | "Ensure unique keys in aDict"					(1 to: 100) detect: [:e |							newKey := title, '-', e asString.							(aDict includesKey: newKey) not].					title := newKey.					assoc := newKey -> assoc value].				aDict add: assoc]]].	^ aDict! !!ObjectExplorer methodsFor: '*morphic' stamp: 'cmm 2/13/2010 19:55'!representsSameBrowseeAs: anotherObjectExplorer	^ self rootObject == anotherObjectExplorer rootObject! !!SystemWindow methodsFor: 'open/close' stamp: 'kb 2/15/2010 11:11'!anyOpenWindowLikeMe		self class reuseWindows ifFalse: [ ^Array empty ].	^ SystemWindow		windowsIn: World 		satisfying: 			[ : each |			each model class = self model class				and: [ (each model respondsTo: #representsSameBrowseeAs:) 				and: [ each model representsSameBrowseeAs: self model ] ] ]! !!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:09'!openInWorld: aWorld	"This msg and its callees result in the window being activeOnlyOnTop"	self anyOpenWindowLikeMe		ifEmpty: 			[ self 				bounds: (RealEstateAgent initialFrameFor: self world: aWorld) ;				openAsIsIn: aWorld ]		ifNotEmptyDo:			[ : windows | 			windows anyOne				expand ;				activate ; 				postAcceptBrowseFor: self ]! !!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:29'!openInWorld: aWorld extent: extent	"This msg and its callees result in the window being activeOnlyOnTop"	^ self anyOpenWindowLikeMe		ifEmpty:			[ self 				position: (RealEstateAgent initialFrameFor: self world: aWorld) topLeft ;				extent: extent.			self openAsIsIn: aWorld ]		ifNotEmptyDo:			[ : windows | 			windows anyOne				expand ;				activate ; 				postAcceptBrowseFor: self ]! !!SystemWindow methodsFor: 'open/close' stamp: 'cmm 2/13/2010 20:07'!postAcceptBrowseFor: anotherSystemWindow	"If I am taking over browsing for anotherSystemWindow, sucblasses may override to, for example, position me to the object to be focused on."	self model postAcceptBrowseFor: anotherSystemWindow model! !!SystemWindow class methodsFor: 'preferences' stamp: 'kb 2/15/2010 11:10'!reuseWindows	<preference: 'Reuse Windows'		category: 'browsing'		description: 'When enabled, before opening a new window check if there is any open window like it, and if there is, reuse it.'		type: #Boolean>	^ReuseWindows ifNil: [ false ]! !!SystemWindow class methodsFor: 'preferences' stamp: 'kb 2/15/2010 11:11'!reuseWindows: aBoolean	ReuseWindows := aBoolean! !MorphicModel subclass: #SystemWindow	instanceVariableNames: 'labelString stripes label closeBox collapseBox activeOnlyOnTop paneMorphs paneRects collapsedFrame fullFrame isCollapsed menuBox mustNotClose labelWidgetAllowance updatablePanes allowReframeHandles labelArea expandBox'	classVariableNames: 'CloseBoxImage CollapseBoxImage ExpandBoxImage MenuBoxImage ReuseWindows TopWindow'	poolDictionaries: ''	category: 'Morphic-Windows'!