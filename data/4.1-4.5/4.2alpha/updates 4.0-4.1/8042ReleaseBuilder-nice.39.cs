"Change Set:		8042ReleaseBuilder-nice.39ReleaseBuilder-nice.39:Use #keys rather than #fasterKeysNote that pattern (x keys asArray sort) could as well be written (x keys sort) now that keys returns an Array...This #asArray is here solely for cross-dialect/fork compatibility."!!ReleaseBuilder methodsFor: 'squeakland' stamp: 'ar 9/27/2005 21:45'!makeSqueaklandReleasePhaseCleanup	"ReleaseBuilder new makeSqueaklandReleasePhaseCleanup"	Smalltalk at: #Browser ifPresent:[:br| br initialize].	ChangeSet 		removeChangeSetsNamedSuchThat: [:cs | cs name ~= ChangeSet current name].	ChangeSet current clear.	ChangeSet current name: 'Unnamed1'.	Smalltalk garbageCollect.	"Reinitialize DataStream; it may hold on to some zapped entitities"	DataStream initialize.	"Remove existing player references"	References keys do: [:k | References removeKey: k].	Smalltalk garbageCollect.	ScheduledControllers := nil.	Behavior flushObsoleteSubclasses.	Smalltalk		garbageCollect;		garbageCollect.	SystemNavigation default obsoleteBehaviors isEmpty 		ifFalse: [self error: 'Still have obsolete behaviors'].	"Reinitialize DataStream; it may hold on to some zapped entitities"	DataStream initialize.	Smalltalk fixObsoleteReferences.	Smalltalk abandonTempNames.	Smalltalk zapAllOtherProjects.	Smalltalk forgetDoIts.	Smalltalk flushClassNameCache.	3 timesRepeat: 			[Smalltalk garbageCollect.			Symbol compactSymbolTable]! !!ReleaseBuilderFor3dot10 methodsFor: 'squeakThreeTen' stamp: 'edc 4/6/2007 06:57'!makeSqueakThreeTenReleasePhaseCleanup	"ReleaseBuilderFor3dot10 new makeSqueakThreeTenReleasePhaseCleanup"| newVersion |	Smalltalk at: #Browser ifPresent:[:br| br initialize].self cleanUnwantedCs.	"Remove existing player references"	References keys do: [:k | References removeKey: k].	Smalltalk garbageCollect.	ScheduledControllers := nil.	Behavior flushObsoleteSubclasses.	SystemNavigation default obsoleteBehaviors isEmpty 		ifFalse: [self error: 'Still have obsolete behaviors'].	"Reinitialize DataStream; it may hold on to some zapped entitities"	DataStream initialize.	self fixObsoleteReferences.	"Smalltalk abandonTempNames."	Smalltalk zapAllOtherProjects.	Smalltalk forgetDoIts.	Smalltalk flushClassNameCache.	3 timesRepeat: 			[Smalltalk garbageCollect.			Symbol compactSymbolTable]."SystemVersion current registerUpdate: 7069." "We only need for when start the release"	newVersion := 'Squeak3.10alpha.' , SystemVersion currenthighestUpdate printString. 		newVersion := newVersion ,'.'.	(SourceFiles at: 2) ifNotNil:		[SmalltalkImage current closeSourceFiles; "so copying thechanges file will always work"			 saveChangesInFileNamed: (SmalltalkImage currentfullNameForChangesNamed: newVersion)].	SmalltalkImage current saveImageInFileNamed: (SmalltalkImage currentfullNameForImageNamed: newVersion)		! !!ReleaseBuilder methodsFor: 'squeakland' stamp: 'ar 9/27/2005 21:45'!makeSqueaklandReleasePhasePrepare	"ReleaseBuilder new makeSqueaklandReleasePhasePrepare"	Undeclared removeUnreferencedKeys.	StandardScriptingSystem initialize.	Preferences initialize.	"(Object classPool at: #DependentsFields) size > 1 ifTrue: [self error:'Still have dependents']."	Undeclared isEmpty ifFalse: [self error:'Please clean out Undeclared'].	"Dump all projects"	Project allSubInstancesDo:[:prj| prj == Project current ifFalse:[Project deletingProject: prj]].	"Set new look so we don't need older fonts later"	StandardScriptingSystem applyNewEToyLook.	Smalltalk at: #Browser ifPresent:[:br| br initialize].	ScriptingSystem deletePrivateGraphics.	ChangeSet removeChangeSetsNamedSuchThat:		[:cs| cs name ~= ChangeSet current name].	ChangeSet current clear.	ChangeSet current name: 'Unnamed1'.	Smalltalk garbageCollect.	"Reinitialize DataStream; it may hold on to some zapped entitities"	DataStream initialize.	"Remove existing player references"	References keys do:[:k| References removeKey: k].	Smalltalk garbageCollect.	ScheduledControllers := nil.	Smalltalk garbageCollect.! !!ReleaseBuilderFor3dot10 methodsFor: 'squeakThreeTen' stamp: 'edc 4/5/2007 07:12'!makeSqueakThreeTenReleasePhasePrepare	"ReleaseBuilderFor3dot10 new makeSqueakThreeTenReleasePhasePrepare"	Undeclared removeUnreferencedKeys.	StandardScriptingSystem initialize.	Preferences initialize.	"(Object classPool at: #DependentsFields) size > 1 ifTrue: [selferror:'Still have dependents']."	Undeclared isEmpty ifFalse: [self error:'Please clean outUndeclared'].	"Dump all projects"	Project allSubInstancesDo:[:prj| prj == Project currentifFalse:[Project deletingProject: prj]].	"Set new look so we don't need older fonts later"	StandardScriptingSystem applyNewEToyLook.	Smalltalk at: #Browser ifPresent:[:br| br initialize].	ScriptingSystem deletePrivateGraphics.	self cleanUnwantedCs.	"Reinitialize DataStream; it may hold on to some zapped entitities"	DataStream initialize.	"Remove existing player references"	References keys do:[:k| References removeKey: k].	Smalltalk garbageCollect.	ScheduledControllers := nil.	Smalltalk garbageCollect.! !