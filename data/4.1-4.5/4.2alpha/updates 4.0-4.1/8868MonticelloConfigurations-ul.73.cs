"Change Set:		8868MonticelloConfigurations-ul.73MonticelloConfigurations-ul.73:- code critics"!!MCConfiguration methodsFor: 'accessing' stamp: 'ul 1/11/2010 07:19'!log	"Answer the receiver's log. If no log exist use the default log"		^log ifNil: [		(name notNil and: [ self class logToFile ]) ifFalse: [ ^Transcript ].		self log: ((FileStream fileNamed: self logFileName) setToEnd; yourself).		log ]! !