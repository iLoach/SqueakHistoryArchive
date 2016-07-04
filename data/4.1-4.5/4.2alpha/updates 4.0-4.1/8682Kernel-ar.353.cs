"Change Set:		8682Kernel-ar.353Kernel-ar.353:Prepare to push the trait composition into TraitOrganizer to avoid duplication in three places (Class, Metaclass, TraitDescription)."!!Metaclass methodsFor: 'accessing' stamp: 'ar 12/30/2009 02:50'!traitComposition: aTraitComposition	super traitComposition: aTraitComposition.	traitComposition := aTraitComposition! !!Class methodsFor: 'accessing' stamp: 'ar 12/30/2009 02:50'!traitComposition: aTraitComposition	super traitComposition: aTraitComposition.	traitComposition := aTraitComposition! !!ClassDescription methodsFor: 'organization' stamp: 'ar 12/30/2009 02:57'!zapOrganization	"Remove the organization of this class by message categories.	This is typically done to save space in small systems.  Classes and methods	created or filed in subsequently will, nonetheless, be organized"	self hasTraitComposition ifFalse:[		self organization: nil.		self isClassSide ifFalse: [self classSide zapOrganization]	].! !!ClassDescription methodsFor: 'initialize-release' stamp: 'ar 12/30/2009 02:57'!obsolete	"Make the receiver obsolete."	self hasTraitComposition ifTrue: [		self traitComposition traits do: [:each |			each removeUser: self]].	superclass removeSubclass: self.	self organization: nil.	super obsolete.! !!Behavior methodsFor: 'initialization' stamp: 'ar 12/30/2009 02:56'!obsolete	"Invalidate and recycle local methods,	e.g., zap the method dictionary if can be done safely."	self canZapMethodDictionary		ifTrue: [self methodDict: self emptyMethodDictionary].! !