"Battery"

"make sure the superclass exists"
#TwoLeadedObject lookupClass!

TwoLeadedObject subclass: #Battery
	instanceVariableNames: 'internalVoltage text'
	classVariableNames: ''
	poolDictionaries: ''
	category: 'Prototypes'!

Battery prototype primitives: 'internalVoltage'; parts: 'text'.
Battery prototype lead1 node primitiveSet.location: 40@10.
Battery prototype lead2 node primitiveSet.location: 40@110.
Battery prototype field: #internalVoltage replaceWith: 1.5 .
Battery prototype field: #text replaceWith: #TextThing clonePrototype.
Battery prototype text primitiveSet.text: '1.5' asText.
Battery prototype text primitiveSet.frame: (55@50 corner: 100@70)!

Constraint owner: Battery prototype
	rule: 'lead1 node voltage-lead2 node voltage = internalVoltage contents'
	methods: 
		#('lead1 node primitiveSet.voltage: lead2 node voltage + internalVoltage'
		'lead2 node primitiveSet.voltage: lead1 node voltage - internalVoltage'
		'internalVoltage reference')!

Constraint owner: Battery prototype
	rule: 'text text = internalVoltage printString asText'
	methods: #('self primitiveSet.internalVoltage: text text asNumber'
		'text text reference')!

Constraint owner: Battery prototype
	rule: 'text frame origin = lead1 node location + lead2 node location /2 + (15@-10)'
	methods: #('text primitiveSet.frame: ((lead1 node location + lead2 node location /2 + (15@-10))
			extent: (45@20))'
		'lead1 node location reference'
		'lead2 node location reference')!

Battery prototype picture: 
	 (Form
	extent: 31@69
	fromArray: #( 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 385 1 385 1 2017 1 2017 1 385 1 385 1 1 1 1 1 1 1 1 1 1 1 1 1 65535 65535 0 1 0 1 0 1 0 1 0 1 1023 65409 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 961 1 961 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1)
	offset: -15@-35)
	origin: 'lead1 node location' asPath!

Battery compile: 
'showPicture: medium | center |
	super showPicture: medium.
	center_lead1 node location+lead2 node location//2.
	PictureForm displayOn: medium at: center rule: Form paint.
	medium drawLine: (Form dotOfSize: 1) from: center-(0@35) to: lead1 node location
		rule: Form over fillColor: nil.
	medium drawLine: (Form dotOfSize: 1) from: center+(0@34) to: lead2 node location
		rule: Form over fillColor: nil.'
classified: 'showing'!
