#include <iostream>
#include <string>
#include <unordered_map>
#include <vector>
#include <algorithm>

class Item {
public:
    std::string name;
    std::string description;

    Item(const std::string& name, const std::string& description)
        : name(name), description(description) {
    }

    void Inspect() const {
        std::cout << description << std::endl;
    }
};

class Room {
public:
    std::string name;
    std::string description;
    std::unordered_map<std::string, Room*> exits;
    std::vector<Item> items;

    Room(const std::string& name, const std::string& description)
        : name(name), description(description) {
    }

    void Describe() const {
        std::cout << name << ": " << description << std::endl;
        if (!items.empty()) {
            std::cout << "You see:" << std::endl;
            for (const auto& item : items) {
                std::cout << "- " << item.name << std::endl;
            }
        }
        if (!exits.empty()) {
            std::cout << "Exits:" << std::endl;
            for (const auto& pair : exits) {
                std::cout << "- " << pair.first << std::endl;
            }
        }
    }
};

class Game {
private:
    Room* currentRoom;
    std::unordered_map<std::string, Room*> rooms;
    std::vector<Item> inventory;
    const int InventoryLimit = 10;

public:
    Game() {
        auto foyer = new Room("Foyer", "A dimly lit entrance hall with a dusty chandelier.");
        auto library = new Room("Library", "Shelves filled with ancient books cover the walls.");
        auto diningHall = new Room("Dining Hall", "A long table sits beneath a cracked stained-glass window.");

        // Foyer Items
        foyer->items = {
            {"Umbrella", "A black umbrella, slightly damp."},
            {"Coat Rack", "An ornate wooden rack with a single coat hanging."},
            {"Welcome Mat", "A mat that says 'WELCOME' in faded letters."},
            {"Dusty Mirror", "A tall mirror covered in a layer of dust."},
            {"Old Key", "A small brass key with intricate carvings."},
            {"Candle", "A candle that has almost melted down to the base."},
            {"Letter", "A sealed letter addressed to no one in particular."},
            {"Hat", "A stylish fedora that has seen better days."},
            {"Boots", "A pair of muddy boots."},
            {"Painting", "A portrait of a stern-looking gentleman."},
            {"Vase", "A cracked vase with dried flowers."}
        };

        // Library Items
        library->items = {
            {"Ancient Book", "An old leather-bound book with strange symbols on its cover."},
            {"Magnifying Glass", "A brass-handled magnifying glass."},
            {"Reading Lamp", "A small green-shaded lamp flickering slightly."},
            {"Scroll", "A rolled-up parchment with illegible text."},
            {"Feather Pen", "A black feathered pen with a silver tip."},
            {"Notebook", "A notebook filled with cryptic notes."},
            {"Globe", "A dusty globe, slightly faded."},
            {"Telescope", "A short telescope aimed at the ceiling."},
            {"Bookshelf", "A massive bookshelf, some books seem loose."},
            {"Ink Bottle", "A half-full bottle of blue ink."},
            {"Rug", "A finely woven rug, frayed at the edges."}
        };

        // Dining Hall Items
        diningHall->items = {
            {"Silver Spoon", "A slightly tarnished silver spoon."},
            {"Tablecloth", "A white cloth with a red wine stain in the center."},
            {"Candle Holder", "A heavy iron candle holder with wax drippings."},
            {"Goblet", "A golden goblet with jewels around the rim."}
        };

        // Connect Rooms
        foyer->exits["north"] = library;
        library->exits["south"] = foyer;

        foyer->exits["west"] = diningHall;
        diningHall->exits["east"] = foyer;

        // Store Rooms
        rooms["Foyer"] = foyer;
        rooms["Library"] = library;
        rooms["Dining Hall"] = diningHall;

        currentRoom = foyer;
    }

    void Start() {
        std::cout << "Welcome to the adventure game!" << std::endl;
        currentRoom->Describe();
        std::string input;

        while (true) {
            std::cout << "\n> ";
            std::getline(std::cin, input);
            std::transform(input.begin(), input.end(), input.begin(), ::tolower);
            if (!ProcessCommand(input)) break;
        }
    }

private:
    bool ProcessCommand(const std::string& command) {
        if (command == "look") {
            currentRoom->Describe();
        }
        else if (command.rfind("inspect ", 0) == 0) {
            std::string itemName = command.substr(8);
            InspectItem(itemName);
        }
        else if (command.rfind("take ", 0) == 0) {
            std::string itemName = command.substr(5);
            TakeItem(itemName);
        }
        else if (command.rfind("drop ", 0) == 0) {
            std::string itemName = command.substr(5);
            DropItem(itemName);
        }
        else if (command == "inventory") {
            ShowInventory();
        }
        else if (command == "quit") {
            std::cout << "Are you sure you want to quit? (yes/no): ";
            std::string response;
            std::getline(std::cin, response);
            std::transform(response.begin(), response.end(), response.begin(), ::tolower);
            return response != "yes";
        }
        else if (IsDirection(command)) {
            Move(command);
        }
        else {
            std::cout << "Unknown command." << std::endl;
        }
        return true;
    }

    bool IsDirection(const std::string& command) {
        return command == "north" || command == "south" || command == "east"
            || command == "west" || command == "up" || command == "down";
    }

    void Move(const std::string& direction) {
        if (currentRoom->exits.count(direction)) {
            currentRoom = currentRoom->exits[direction];
            std::cout << "You move " << direction << "." << std::endl;
            currentRoom->Describe();
        }
        else {
            std::cout << "You can't go that way." << std::endl;
        }
    }

    void InspectItem(const std::string& name) {
        auto it = std::find_if(currentRoom->items.begin(), currentRoom->items.end(),
            [&](const Item& i) { return ToLower(i.name) == name; });

        if (it != currentRoom->items.end()) {
            it->Inspect();
            return;
        }
        auto invIt = std::find_if(inventory.begin(), inventory.end(),
            [&](const Item& i) { return ToLower(i.name) == name; });

        if (invIt != inventory.end()) {
            invIt->Inspect();
        }
        else {
            std::cout << "There is no such item here or in your inventory." << std::endl;
        }
    }

    void TakeItem(const std::string& name) {
        if (inventory.size() >= InventoryLimit) {
            std::cout << "Your inventory is full." << std::endl;
            return;
        }
        auto it = std::find_if(currentRoom->items.begin(), currentRoom->items.end(),
            [&](const Item& i) { return ToLower(i.name) == name; });

        if (it != currentRoom->items.end()) {
            inventory.push_back(*it);
            std::cout << "You take the " << it->name << "." << std::endl;
            currentRoom->items.erase(it);
        }
        else {
            std::cout << "There is no such item here." << std::endl;
        }
    }

    void DropItem(const std::string& name) {
        auto it = std::find_if(inventory.begin(), inventory.end(),
            [&](const Item& i) { return ToLower(i.name) == name; });

        if (it != inventory.end()) {
            currentRoom->items.push_back(*it);
            std::cout << "You drop the " << it->name << "." << std::endl;
            inventory.erase(it);
        }
        else {
            std::cout << "You don't have that item." << std::endl;
        }
    }

    void ShowInventory() const {
        if (inventory.empty()) {
            std::cout << "You are not carrying anything." << std::endl;
        }
        else {
            std::cout << "You are carrying:" << std::endl;
            for (const auto& item : inventory) {
                std::cout << "- " << item.name << std::endl;
            }
        }
    }

    std::string ToLower(const std::string& str) const {
        std::string lower = str;
        std::transform(lower.begin(), lower.end(), lower.begin(), ::tolower);
        return lower;
    }
};

int main() {
    Game game;
    game.Start();
    return 0;
}
