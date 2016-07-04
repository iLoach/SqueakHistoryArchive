"Change Set:		8194Monticello-ar.333Monticello-ar.333:Remove progress bar from the core installation loop since it can call methods that aren't installed yet."!!MCPackageLoader methodsFor: 'private' stamp: 'ar 11/18/2009 21:21'!basicLoad	errorDefinitions := OrderedCollection new.	[["Pass 1: Load everything but the methods,  which are collected in methodAdditions."	additions do: [:ea | 		[ea isMethodDefinition 			ifTrue:[methodAdditions add: ea asMethodAddition]			ifFalse:[ea load]]on: Error do: [errorDefinitions add: ea].	] displayingProgress: 'Reshaping classes...'.	"Pass 2: We compile new / changed methods"	methodAdditions do:[:ea| ea createCompiledMethod] displayingProgress: 'Compiling...'.	'Installing...' displayProgressAt: Sensor cursorPoint from: 0 to: 2 during:[:bar|		"There is progress *during* installation since a progress bar update		will redraw the world and potentially call methods that we're just trying to install."		bar value: 1.		"Pass 3: Install the new / changed methods		(this is a separate pass to allow compiler changes to be loaded)"		methodAdditions do:[:ea| ea installMethod].		"Pass 4: Remove the obsolete methods"		removals do:[:ea| ea unload].	].	"Try again any delayed definitions"	self shouldWarnAboutErrors ifTrue: [self warnAboutErrors].	errorDefinitions do: [:ea | ea load] displayingProgress: 'Reloading...'.	"Finally, notify observers for the method additions"	methodAdditions do: [:each | each notifyObservers].	additions do: [:ea | ea postloadOver: (self obsoletionFor: ea)] displayingProgress: 'Initializing...'	] on: InMidstOfFileinNotification do: [:n | n resume: true]	] ensure: [self flushChangesFile]! !