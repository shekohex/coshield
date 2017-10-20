#pragma once
class BotProperties
{
public: static int SendPacketPatchAddress, SendPacketAddress, NetworkClass;
public: static int ReceivePacketPatchAddress, ReceivePacketAddress;
public: static int DisconnectAddress, FarJumpAddress;
public: static std::string ClientType;
public: static int JumpAddress, WalkAddress, WalkJumpClass;
public: static int MapDataAddress, PathFindAddress;

public:
	BotProperties(void);
	~BotProperties(void);
};

