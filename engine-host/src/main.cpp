#include <iostream>
#include <string_view>

#ifndef RCC_VERSION
#define RCC_VERSION "0.01.03"
#endif

namespace {
void PrintUsage() {
    std::cout << "Reach Community Client Engine Host " << RCC_VERSION << "\n"
              << "Usage:\n"
              << "  rcc-engine-host --version\n"
              << "  rcc-engine-host --self-check\n"
              << "  rcc-engine-host --capabilities\n";
}

void PrintCapabilities() {
    std::cout << R"json({
  "protocolVersion": 1,
  "hostVersion": ")json" << RCC_VERSION << R"json(",
  "engineRuntime": "not_implemented",
  "capabilities": {
    "originalReachMainMenu": "requires_rcc_engine_runtime",
    "communityServerBrowser": "requires_rcc_community_service",
    "multiplayerSessions": "requires_rcc_engine_runtime",
    "dedicatedServer": "not_implemented"
  },
  "externalGameClientDependency": false,
  "gameFilesBundled": false,
  "antiCheatOrDrmBypass": false
}
)json";
}
} // namespace

int main(int argc, char** argv) {
    if (argc != 2) {
        PrintUsage();
        return argc == 1 ? 0 : 2;
    }

    const std::string_view command{argv[1]};
    if (command == "--version") {
        std::cout << RCC_VERSION << '\n';
        return 0;
    }
    if (command == "--capabilities") {
        PrintCapabilities();
        return 0;
    }
    if (command == "--self-check") {
        std::cout << "RCC engine-host scaffold is operational.\n"
                  << "No Halo: Reach engine module is loaded by this build.\n"
                  << "No external game client is used or launched.\n";
        return 0;
    }

    PrintUsage();
    return 2;
}
