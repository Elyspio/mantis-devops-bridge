const {spawnSync} = require("child_process");

spawnSync("docker", ["buildx", "bake", "--push", "--pull"], {cwd: __dirname, stdio: "inherit"});
spawnSync("ssh", ["elyspio@192.168.0.59", "cd /apps/coexya/mantis-devops-bridge && docker compose pull && docker compose up -d --force-recreate"], {
    cwd: __dirname,
    stdio: "inherit"
});
