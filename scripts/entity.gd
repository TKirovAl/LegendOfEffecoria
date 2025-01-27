extends Resource

class Entity:
	var name: String
	var health: int
	var attack: int
	
	func _init(name: String, health: int, attack: int):
		self.name = name
		self.health = health
		self.attack = attack
	
	func attack_player():
		print(self.name + " атакует на" + str(self.attack) + " урона!")
		
		
