"Change Set:		8468Monticello-ar.338Monticello-ar.338:Try hard not to remove methods that have been moved to a different package."!!MCMethodDefinition methodsFor: 'installing' stamp: 'ar 12/12/2009 17:05'!removeSelector: aSelector fromClass: aClass	"Safely remove the given selector from the target class.	Be careful not to remove the selector when it has wondered	to another package."	| newCategory |	newCategory := aClass organization categoryOfElement: aSelector.	newCategory ifNotNil:[		"If moved to and fro extension, ignore removal"		(category beginsWith: '*') = (newCategory beginsWith: '*') ifFalse:[^self].		"Check if moved between different extension categories"		((category beginsWith: '*') and:[category ~= newCategory]) ifTrue:[^self]].	aClass removeSelector: aSelector.! !!MCMethodDefinition methodsFor: 'installing' stamp: 'ar 12/12/2009 15:40'!unload	| previousVersion |	self isOverrideMethod ifTrue: [previousVersion := self scanForPreviousVersion].	previousVersion		ifNil: [self actualClass ifNotNil:[:class| self removeSelector: selector fromClass: class]]		ifNotNil: [previousVersion fileIn] ! !