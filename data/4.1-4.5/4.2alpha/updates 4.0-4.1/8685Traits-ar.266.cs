"Change Set:		8685Traits-ar.266Traits-ar.266:Also remove Trait class>>initialize.Traits-ar.265:Remove accessors for traitComposition."!!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 12/30/2009 03:24'!traitComposition: aTraitComposition	"Install my trait composition"	aTraitComposition isEmptyOrNil ifTrue:[		self organization isTraitOrganizer 			ifTrue:[self organization: (ClassOrganizer newFrom: self organization)].	] ifFalse:[		self organization isTraitOrganizer 			ifFalse:[self organization: (TraitOrganizer newFrom: self organization)].		self organization traitComposition: aTraitComposition.	].! !Trait class removeSelector: #initialize!TraitDescription removeSelector: #traitComposition!TraitDescription removeSelector: #traitComposition:!Trait initialize!