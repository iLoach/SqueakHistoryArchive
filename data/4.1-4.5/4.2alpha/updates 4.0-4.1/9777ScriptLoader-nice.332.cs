"Change Set:		9777ScriptLoader-nice.332ScriptLoader-nice.332:Fix underscores"!!ScriptLoader methodsFor: 'private helpers' stamp: 'dvf 10/14/2005 09:17'!loadTogether: aCollection merge: aBoolean	| loader |	loader := aBoolean		ifTrue: [ MCVersionMerger new ]		ifFalse: [ MCVersionLoader new].	(self newerVersionsIn: aCollection)		do: [:fn | loader addVersion: (self repository loadVersionFromFileNamed: fn)]  	  	displayingProgress: 'Adding versions...'.	aBoolean		ifTrue: [[loader merge] on: MCMergeResolutionRequest do: [:request |					request merger conflicts isEmpty						ifTrue: [request resume: true]						ifFalse: [request pass]]]		ifFalse: [loader load]! !!ScriptLoader methodsFor: 'private helpers' stamp: 'md 2/16/2006 16:43'!mergePackagesNamed: names	| vm  |	repository := MCHttpRepository                location: 'http://source.squeakfoundation.org/39a'                user: ''                password: ''.	vm := MCVersionMerger new.	names		do: [:fn | vm addVersion: (repository loadVersionFromFileNamed: fn)]		displayingProgress: 'Adding versions...'.	[vm merge]		on: MCMergeResolutionRequest do: [:request |			request merger conflicts isEmpty				ifTrue: [request resume: true]				ifFalse: [request pass]]! !