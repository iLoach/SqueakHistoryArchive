"Change Set:		9614ShoutCore-ar.12ShoutCore-ar.12:Avoid dictionary protocol in Smalltalk."!!SHMCClassDefinition methodsFor: 'act like environment' stamp: 'ar 3/5/2010 20:38'!hasBindingThatBeginsWith: aString	(Smalltalk globals hasBindingThatBeginsWith: aString) ifTrue: [^true].	items do:[:each |		(each isClassDefinition and: [each className beginsWith: aString])			ifTrue:[^true]].	^false! !