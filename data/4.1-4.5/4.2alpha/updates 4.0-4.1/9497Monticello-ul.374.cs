"Change Set:		9497Monticello-ul.374Monticello-ul.374:- replaced MCMethodDefinition's Definitions class variable with a class instance variable. The cached definitions are no longer registered for finalization.- a bit of cleanup around MCDefinition's Instances class variableMonticello-ar.372:Cleanup for MCDefinition, MCFileBasedRepository, MCMethodDefinition."!MCDefinition subclass: #MCMethodDefinition	instanceVariableNames: 'classIsMeta source category selector className timeStamp'	classVariableNames: ''	poolDictionaries: ''	category: 'Monticello-Modeling'!!MCMethodDefinition class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:03'!cleanUp	"Flush caches"	self shutDown.! !!MCMethodDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 17:42'!shutDown		definitions := nil.! !!MCFileBasedRepository class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:03'!cleanUp	"Flush caches"	self flushAllCaches.! !!MCDefinition class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:03'!cleanUp	"Flush caches"	self clearInstances.! !!MCMethodDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 17:42'!cachedDefinitions		^definitions ifNil: [ definitions := WeakIdentityKeyDictionary new ]! !!MCDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 17:45'!instanceLike: aDefinition	^(Instances ifNil: [ Instances := WeakSet new ])		like: aDefinition		ifAbsent: [ Instances add: aDefinition ]! !!MCDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 17:45'!clearInstances	Instances := nil! !MCSnapshotBrowser removeSelector: #aboutToStyle:!