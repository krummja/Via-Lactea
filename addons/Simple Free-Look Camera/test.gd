extends Node


onready var recipientText
onready var maxLength

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _input(event):
	var just_pressed = event.is_pressed() and not event.is_echo()

	if recipientText.length() < maxLength:
		if Input.is_key_pressed(KEY_A) && just_pressed:
			recipientText += "a"


