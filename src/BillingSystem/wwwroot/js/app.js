async function checkHealth() {
  const out = document.getElementById("health");
  try {
    const res = await fetch("/api/health");
    out.textContent = JSON.stringify(await res.json(), null, 2);
  } catch (err) {
    out.textContent = "Server not reachable: " + err.message;
  }
}

checkHealth();