"Change Set:		9360ScriptLoader-nice.331ScriptLoader-nice.331:It is useless to assign block argument with nil"!!ScriptLoader methodsFor: 'cleaning' stamp: 'nice 2/11/2010 17:28'!cleanUpEtoys	"self new cleanUpEtoys"	StandardScriptingSystem removeUnreferencedPlayers.	(self confirm: 'Remove all projects and players?')		ifFalse: [^self].	Project removeAllButCurrent.	#('Morphic-UserObjects' 'EToy-UserObjects' 'Morphic-Imported' )		do: [:each | SystemOrganization removeSystemCategory: each].			Smalltalk		at: #Player		ifPresent: [:superCls | superCls				allSubclassesDo: [:cls | 					cls isSystemDefined						ifFalse: [cls removeFromSystem].					]].! !