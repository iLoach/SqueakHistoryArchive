"Change Set:		8094Morphic-ar.221Morphic-ar.221:Merging Morphic-dtl.220:Add #defaultBackgroundColor (moved from Project to MorphicProject).Move #initMorphic and #InstallPasteUpAsWorld: from Project.Flag #initMorphic for removal (but check if Etoys uses it).Morphic-MarcoSchmidt.217:Remove the typo in TheWorldMenu>>setDisplayDepthsee http://bugs.squeak.org/view.php?id=7408 Morphic-dtl.218:Move docking bars support from Project to MorphicProject, with some cosmetic changes and spelling corrections.Morphic-dtl.219:Move flaps support from Project to MorphicProject.Remove 3 #isMorphic: sends, fix spelling error and comment example.Morphic-dtl.220:Add #defaultBackgroundColor (moved from Project to MorphicProject).Move #initMorphic and #InstallPasteUpAsWorld: from Project.Flag #initMorphic for removal (but check if Etoys uses it).Morphic-ar.220:Undeclared cleanup. Declare DefaultFill in MorphicProject and promote isSafeToServe into Morph to remove FlashMorph reference (requires companion change in Nebraska)."!Project subclass: #MorphicProject	instanceVariableNames: ''	classVariableNames: 'DefaultFill'	poolDictionaries: ''	category: 'Morphic-Support'!!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:21'!suppressFlapsString	^ (self flapsSuppressed		ifTrue: ['<no>']		ifFalse: ['<yes>']), 'show shared tabs (F)' translated! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:21'!showSharedFlaps	"Answer whether shared flaps are shown or suppressed in this project"	| result |	result := Preferences showSharedFlaps.	^ self == Project current		ifTrue:			[result]		ifFalse:			[self projectPreferenceAt: #showSharedFlaps ifAbsent: [result]]! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:22'!showWorldMainDockingBarString	^ (self showWorldMainDockingBar		ifTrue: ['<yes>']		ifFalse: ['<no>'])		, 'show main docking bar (M)' translated! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:20'!assureMainDockingBarPresenceMatchesPreference	"Synchronize the state of the receiver's dockings with the  	preference"	(self showWorldMainDockingBar)		ifTrue: [self createOrUpdateMainDockingBar]		ifFalse: [self removeMainDockingBar]! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:22'!showWorldMainDockingBar: aBoolean 	"Change the receiver to show the main docking bar"	self projectPreferenceFlagDictionary at: #showWorldMainDockingBar put: aBoolean.	(self == Project current			and: [aBoolean ~= Preferences showWorldMainDockingBar])		ifTrue: [Preferences setPreference: #showWorldMainDockingBar toValue: aBoolean].	self assureMainDockingBarPresenceMatchesPreference! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:22'!toggleShowWorldMainDockingBar	self showWorldMainDockingBar: self showWorldMainDockingBar not! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:16'!enableDisableGlobalFlap: aFlapTab	"For the benefit of pre-existing which-global-flap buttons from a design now left behind."	self flag: #toRemove.	^ self inform: 'Sorry, this is an obsolete menu; pleasedismiss it and get a fresh menu.  Thanks.'.! !!MorphicProject methodsFor: 'initialize' stamp: 'dtl 10/31/2009 15:34'!defaultBackgroundColor	^ Preferences defaultWorldColor! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:16'!cleanseDisabledGlobalFlapIDsList	"Make certain that the items on the disabled-global-flap list are actually global flaps, and if not, get rid of them"	| disabledFlapIDs currentGlobalIDs oldList |	disabledFlapIDs := self parameterAt: #disabledGlobalFlapIDs ifAbsent: [Set new].	currentGlobalIDs := Flaps globalFlapTabsIfAny collect: [:f | f flapID].	oldList := Project current projectParameterAt: #disabledGlobalFlaps ifAbsent: [nil].	oldList ifNotNil:		[disabledFlapIDs := oldList select: [:aFlap | aFlap flapID]].	disabledFlapIDs := disabledFlapIDs select: [:anID | currentGlobalIDs includes: anID].	self projectParameterAt: #disabledGlobalFlapIDs put: disabledFlapIDs.	projectParameters ifNotNil:		[projectParameters removeKey: #disabledGlobalFlaps ifAbsent: []].! !!TheWorldMenu methodsFor: 'commands' stamp: 'MarcoSchmidt 10/27/2009 16:46'!setDisplayDepth	"Let the user choose a new depth for the display. "	| result oldDepth allDepths allLabels hasBoth |	oldDepth := Display nativeDepth.	allDepths := #(1 -1 2 -2 4 -4 8 -8 16 -16 32 -32) select: [:d | Display supportsDisplayDepth: d].	hasBoth := (allDepths anySatisfy:[:d| d > 0]) and:[allDepths anySatisfy:[:d| d < 0]].	allLabels := allDepths collect:[:d|		String streamContents:[:s|			s nextPutAll: (d = oldDepth ifTrue:['<on>'] ifFalse:['<off>']).			s print: d abs.			hasBoth ifTrue:[s nextPutAll: (d > 0 ifTrue:['  (big endian)'] ifFalse:['  (little endian)'])].		]].	result := UIManager default		chooseFrom: allLabels 		values: allDepths 		title: 'Choose a display depth' translated.	result ifNotNil: [Display newDepth: result].	oldDepth := oldDepth abs.	(Smalltalk isMorphic and: [(Display depth < 4) ~= (oldDepth < 4)])		ifTrue:			["Repaint windows since they look better all white in depth < 4"			(SystemWindow windowsIn: myWorld satisfying: [:w | true]) do:				[:w |				oldDepth < 4					ifTrue: [w restoreDefaultPaneColor]					ifFalse: [w updatePaneColors]]]! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:20'!globalFlapWithIDEnabledString: aFlapID	"Answer the string to be shown in a menu to represent the status of the given flap regarding whether it it should be shown in this project."	| aFlapTab |	aFlapTab := Flaps globalFlapTabWithID: aFlapID.	^ (self isFlapEnabled: aFlapTab)		ifTrue:			['<on>', aFlapTab wording]		ifFalse:			['<off>', aFlapTab wording]! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:19'!globalFlapEnabledString: aFlapTab	"Answer the string to be shown in a menu to represent the status of the given flap regarding whether it it should be shown in this project."	^ (self isFlapEnabled: aFlapTab)		ifTrue:			['<on>', aFlapTab wording]		ifFalse:			['<off>', aFlapTab wording]! !!MorphicProject methodsFor: 'initialize' stamp: 'dtl 10/31/2009 15:38'!initMorphic	"Written so that Morphic can still be removed.  Note that #initialize is never actually called for a morphic project -- see the senders of this method."	self flag: #toRemove. "check if this method still used by Etoys"	Smalltalk verifyMorphicAvailability ifFalse: [^ nil].	changeSet := ChangeSet new.	transcript := TranscriptStream new.	displayDepth := Display depth.	parentProject := CurrentProject.	isolatedHead := false.	world := PasteUpMorph newWorldForProject: self.	Locale switchToID: CurrentProject localeID.	self initializeProjectPreferences. "Do this *after* a world is installed so that the project will be recognized as a morphic one."	Preferences useVectorVocabulary ifTrue: [world installVectorVocabulary]! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:21'!removeMainDockingBar	"Remove the receiver's main docking bars"	self world mainDockingBars		do: [:each | each delete]! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:19'!showWorldMainDockingBar	^ self projectPreferenceFlagDictionary		at: #showWorldMainDockingBar		ifAbsent: [Preferences showWorldMainDockingBar]! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:21'!isFlapIDEnabled:  aFlapID	"Answer whether a flap of the given ID is enabled in this project"	| disabledFlapIDs  |	disabledFlapIDs := self parameterAt: #disabledGlobalFlapIDs ifAbsent: [^ true].	^ (disabledFlapIDs includes: aFlapID) not! !!MorphicProject methodsFor: 'initialize' stamp: 'dtl 10/31/2009 15:40'!installPasteUpAsWorld: pasteUpMorph	"(ProjectViewMorph newMorphicProjectOn: aPasteUpMorph) openInWorld."	world := pasteUpMorph beWorldForProject: self! !!MorphicProject methodsFor: 'docking bars support' stamp: 'dtl 10/31/2009 14:24'!createOrUpdateMainDockingBar	"Private - create a new main docking bar or update the current one"	| w mainDockingBars |	w := self world.	mainDockingBars := w mainDockingBars.	mainDockingBars isEmpty		ifTrue: ["no docking bar, just create a new one"			TheWorldMainDockingBar instance createDockingBar openInWorld: w.			^ self].	"update if needed"	mainDockingBars		do: [:each | TheWorldMainDockingBar instance updateIfNeeded: each]! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:14'!assureFlapIntegrity	"Make certain that the items on the disabled-global-flap list are actually global flaps, and if not, get rid of them.  Also, old (and damaging) parameters that held references to actual disabled flaps are cleansed"	| disabledFlapIDs currentGlobalIDs oldList |	disabledFlapIDs := self parameterAt: #disabledGlobalFlapIDs ifAbsent: [Set new].	currentGlobalIDs := Flaps globalFlapTabsIfAny collect: [:f | f flapID].	oldList := Project current projectParameterAt: #disabledGlobalFlaps ifAbsent: [nil].	oldList ifNotNil:		[disabledFlapIDs := oldList collect: [:aFlap | aFlap flapID].		disabledFlapIDs addAll: {'Scripting' translated. 'Stack Tools' translated. 'Painting' translated}].	disabledFlapIDs := disabledFlapIDs select: [:anID | currentGlobalIDs includes: anID].	self projectParameterAt: #disabledGlobalFlapIDs put: disabledFlapIDs asSet.	self assureNavigatorPresenceMatchesPreference.	projectParameters ifNotNil:		[projectParameters removeKey: #disabledGlobalFlaps ifAbsent: []]! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:23'!toggleFlapsSuppressed	"Project current toggleFlapsSuppressed"	^self flapsSuppressed: self flapsSuppressed not.! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:17'!flapsSuppressed	"Answer whether flaps are suppressed in this project"	^ self showSharedFlaps not! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:20'!isFlapEnabled:  aFlapTab	"Answer whether the given flap tab is enabled in this project"	^ self isFlapIDEnabled: aFlapTab flapID! !!Morph methodsFor: 'testing' stamp: 'ar 10/31/2009 13:01'!isSafeToServe	"Return true if it is safe to serve this Morph using Nebraska." 	^true! !!MorphicProject methodsFor: 'flaps support' stamp: 'dtl 10/31/2009 15:18'!flapsSuppressed: aBoolean	"Make the setting of the flag that governs whether global flaps are suppressed in the project be as indicated and add or remove the actual flaps"	self projectPreferenceFlagDictionary at: #showSharedFlaps put: aBoolean not.	self == Project current  "Typical case"		ifTrue:			[Preferences setPreference: #showSharedFlaps toValue: aBoolean not]		ifFalse:   "Anomalous case where this project is not the current one."			[aBoolean				ifTrue:							[Flaps globalFlapTabsIfAny do:						[:aFlapTab | Flaps removeFlapTab: aFlapTab keepInList: true]]				ifFalse:					[self currentWorld addGlobalFlaps]].	Project current assureNavigatorPresenceMatchesPreference! !PasteUpMorph removeSelector: #isSafeToServe!