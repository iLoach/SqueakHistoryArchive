"Change Set:		9533Monticello-ul.375Monticello-ul.375:- replaced MCDefinition's Instances class variable with a class instance variable. Smaller WeakSets are better than a large one."!Object subclass: #MCDefinition	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'Monticello-Base'!!MCDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 22:10'!instanceLike: aDefinition	^(instances ifNil: [ instances := WeakSet new ])		like: aDefinition		ifAbsent: [ instances add: aDefinition ]! !!MCDefinition class methodsFor: 'as yet unclassified' stamp: 'ul 2/26/2010 22:10'!clearInstances	instances := nil.	self subclassesDo: #clearInstances! !