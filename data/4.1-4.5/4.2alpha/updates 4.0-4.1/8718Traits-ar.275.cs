"Change Set:		8718Traits-ar.275Traits-ar.275:A bit of refactoring. Break up ClassDescription>>installTraitsFrom: since it had gotten too long. Remove the obsolete definitionST80 protocol. Move updateTraitsFrom: up into ClassDescription.Traits-ar.274:Add post-load initialization for reloading traits."!!Trait class methodsFor: 'load-unload' stamp: 'ar 1/1/2010 20:09'!initialize	"Install after loading"	self install.! !!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 1/2/2010 03:36'!assembleTraitMethodsFrom: aTraitComposition	"Assemble the resulting methods for installing the given trait composition.	Returns a Dictionary instead of a MethodDictionary for speed (MDs grow by #become:)"	| methods oldMethod |	methods := Dictionary new.	"Stick in the local methods first, since this avoids generating conflict methods unnecessarily"	self selectorsAndMethodsDo:[:sel :newMethod|		(self isLocalMethod: newMethod)			ifTrue:[methods at: sel put:newMethod]].	"Now assemble the traits methods"	aTraitComposition do:[:trait|		trait selectorsAndMethodsDo:[:sel :newMethod|			oldMethod := methods at: sel ifAbsentPut:[newMethod].			newMethod == oldMethod ifFalse:["a conflict"				(self isLocalMethod: oldMethod) ifFalse:[					methods at: sel put: (self resolveTraitsConflict: sel from: oldMethod to: newMethod)]]]].	^methods! !!Trait class methodsFor: 'load-unload' stamp: 'ar 1/1/2010 20:09'!install	"Make me the default Trait implementation"	ClassDescription traitImpl: self.	"And restore any previously flattened traits"	self restoreAllTraits.! !!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 1/2/2010 03:50'!installTraitMethodDict: methods	"After having assembled the trait composition, install its methods."	| oldCategories oldMethod removals |	"Apply the changes. We first add the new or changed methods."	oldCategories := Set new.	methods keysAndValuesDo:[:sel :newMethod|		oldMethod := self compiledMethodAt: sel ifAbsent:[nil].		oldMethod == newMethod ifFalse:[			self traitAddSelector: sel withMethod: newMethod.			(self organization categoryOfElement: sel) ifNotNil:[:cat| oldCategories add: cat].			self organization classify: sel under: 				(newMethod methodHome organization categoryOfElement: newMethod selector)]].	"Now remove the old or obsoleted ones"	removals := OrderedCollection new.	self selectorsDo:[:sel| (methods includesKey: sel) ifFalse:[removals add: sel]].	removals do:[:sel| self traitRemoveSelector: sel].	"Clean out empty categories"	oldCategories do:[:cat|		(self organization isEmptyCategoryNamed: cat)			ifTrue:[self organization removeCategory: cat]].! !!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 1/2/2010 03:38'!traitComposition: aTraitComposition	"Install my trait composition"	self traitComposition do:[:tc|  tc removeTraitUser: self].	aTraitComposition isEmptyOrNil ifTrue:[		self organization isTraitOrganizer 			ifTrue:[self organization: (ClassOrganizer newFrom: self organization)].	] ifFalse:[		self organization isTraitOrganizer 			ifFalse:[self organization: (TraitOrganizer newFrom: self organization)].		self organization traitComposition: aTraitComposition.		aTraitComposition do:[:tc| tc addTraitUser: self].	].! !!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 1/2/2010 03:50'!installTraitsFrom: aTraitComposition	"Install the traits from the given composition. This method implements	the core composition method - all others are just optimizations for	particular cases. Consequently, the optimized versions can always fall	back to this method when things get too hairy."	| allTraits methods |	(self traitComposition isEmpty and: [aTraitComposition isEmpty]) ifTrue: [^self].	"Check for cycles"	allTraits := aTraitComposition gather: [:t | t allTraits copyWith: t].	(allTraits includes: self) ifTrue:[^self error: 'Cyclic trait definition detected'].	self traitComposition: aTraitComposition.	methods := self assembleTraitMethodsFrom: aTraitComposition.	self installTraitMethodDict: methods.	self isMeta ifFalse:[self classSide updateTraitsFrom: aTraitComposition].! !!ClassDescription methodsFor: '*Traits-NanoKernel' stamp: 'ar 1/2/2010 03:55'!updateTraitsFrom: instanceTraits	"ClassTrait/Metaclass only. Update me from the given instance traits"	| map newTraits trait |	self isMeta ifFalse:[self error: 'This is a metaclass operation'].	map := Dictionary new.	self traitComposition do:[:composed| map at: composed trait put: composed].	newTraits := (instanceTraits collect:[:composed|		trait := composed trait classTrait.		map at: trait ifAbsent:[trait]] 	), (self traitComposition select:[:comp| comp trait isBaseTrait]).	self installTraitsFrom: newTraits! !ClassTrait removeSelector: #updateTraitsFrom:!Metaclass removeSelector: #updateTraitsFrom:!Trait removeSelector: #definitionST80!ClassTrait removeSelector: #definitionST80!Trait initialize!