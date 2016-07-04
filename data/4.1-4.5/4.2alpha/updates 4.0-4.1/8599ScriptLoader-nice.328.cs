"Change Set:		8599ScriptLoader-nice.328ScriptLoader-nice.328:Cosmetic: move or remove a few temps inside closures"!!ScriptLoader methodsFor: 'private helpers' stamp: 'nice 12/27/2009 03:11'!loadOneAfterTheOther: aCollection merge: aBoolean		(self newerVersionsIn: aCollection)		do: [:fn | | loader |			loader := aBoolean				ifTrue: [ MCVersionMerger new ]				ifFalse: [ MCVersionLoader new].			loader addVersion: (self repository loadVersionFromFileNamed: fn).			aBoolean				ifTrue: [[loader merge] on: MCMergeResolutionRequest do: [:request |							request merger conflicts isEmpty								ifTrue: [request resume: true]								ifFalse: [request pass]]]				ifFalse: [loader load]]  	  	displayingProgress: 'Loading versions...'.	! !!ScriptLoader methodsFor: 'cleaning' stamp: 'nice 12/27/2009 03:11'!fixObsoleteReferences	"self new fixObsoleteReferences"		Preference allInstances do: [:each | | informee | 		informee := each instVarNamed: #changeInformee.		((informee isKindOf: Behavior)			and: [informee isObsolete])			ifTrue: [				Transcript show: each name; cr.				each instVarNamed: #changeInformee put: (Smalltalk at: (informee name copyReplaceAll: 'AnObsolete' with: '') asSymbol)]]. 	CompiledMethod allInstances do: [:method |		| obsoleteBindings |		obsoleteBindings := method literals select: [:literal |			literal isVariableBinding				and: [literal value isBehavior]				and: [literal value isObsolete]].		obsoleteBindings do: [:binding |			| obsName realName realClass |			obsName := binding value name.			Transcript show: obsName; cr.			realName := obsName copyReplaceAll: 'AnObsolete' with: ''.			realClass := Smalltalk at: realName asSymbol ifAbsent: [UndefinedObject].			binding isSpecialWriteBinding				ifTrue: [binding privateSetKey: binding key value: realClass]				ifFalse: [binding key: binding key value: realClass]]].	Behavior flushObsoleteSubclasses.	Smalltalk garbageCollect; garbageCollect.	SystemNavigation default obsoleteBehaviors size > 0		ifTrue: [SystemNavigation default obsoleteBehaviors inspect]! !