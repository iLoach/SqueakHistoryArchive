"Change Set:		8129Monticello-ul.331Monticello-ul.331:- faster hashing with String >> #hashWithInitialHash:. Load  Collections-ul.179 before this package."!!MCTraitDefinition methodsFor: 'comparing' stamp: 'ul 11/2/2009 04:55'!hash	| hash |	hash := name hashWithInitialHash: 0.	hash := self traitCompositionString hashWithInitialHash: hash.	hash := (category ifNil: ['']) hashWithInitialHash: hash.	^hash! !!MCClassDefinition methodsFor: 'comparing' stamp: 'ul 11/2/2009 03:32'!hash	| hash |	hash := name hashWithInitialHash: 0.	hash := superclassName hashWithInitialHash: hash.	hash := self traitCompositionString hashWithInitialHash: hash.	hash := self classTraitComposition asString hashWithInitialHash: hash.	hash := (category ifNil: ['']) hashWithInitialHash: hash.	hash := type hashWithInitialHash: hash.	variables do: [ :v |		hash := v name hashWithInitialHash: hash ].	^hash! !!MCMethodDefinition methodsFor: 'comparing' stamp: 'ul 11/2/2009 03:33'!hash	| hash |	hash := classIsMeta asString hashWithInitialHash: 0.	hash := source hashWithInitialHash: hash.	hash := category hashWithInitialHash: hash.	hash := className hashWithInitialHash: hash.	^ hash! !!MCClassTraitDefinition methodsFor: 'accessing' stamp: 'ul 11/2/2009 03:32'!hash	| hash |	hash := baseTrait hashWithInitialHash: 0.	hash := self classTraitCompositionString hashWithInitialHash: hash.	^hash! !