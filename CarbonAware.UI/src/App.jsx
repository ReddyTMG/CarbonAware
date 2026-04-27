import { useEffect, useState } from 'react'
import * as signalR from '@microsoft/signalr'

function App() {
  const [logs, setLogs] = useState([]);
  const [loading, setLoading] = useState(true); // Keep the loading state

  useEffect(() => {
    // 1. Initial Fetch
    fetch('http://localhost:5258/carbon/history')
      .then(res => res.json())
      .then(data => {
        setLogs(data);
        setLoading(false); // Stop loading once history is in
      })
      .catch(err => {
        console.error("Fetch error:", err);
        setLoading(false);
      });

    // 2. SignalR Connection
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5258/carbonHub")
      .withAutomaticReconnect()
      .build();

    // Optimized start function
    const startSignalR = async () => {
      try {
        await connection.start();
        console.log("Connected to SignalR Hub!");

        connection.on("ReceiveNewLog", (newLog) => {
          setLogs(prevLogs => {
            // 1. Check if we already have this ID in our list
            const isDuplicate = prevLogs.some(log => log.id === newLog.id);

            // 2. If it's a duplicate, don't change anything
            if (isDuplicate) return prevLogs;

            // 3. If it's new, add it to the top and keep the list size at 20
            return [newLog, ...prevLogs.slice(0, 19)];
          });
        });
      } catch (err) {
        // Ignore the error if it was just a manual stop/abort from React's lifecycle
        if (err.name !== 'AbortError') {
          console.error("SignalR Connection Error: ", err);
        }
      }
    };

    startSignalR();

    return () => {
      // Only stop if we are actually connected
      if (connection.state === signalR.HubConnectionState.Connected) {
        connection.stop();
      }
    };
  }, []);

  return (
    <div className="p-8">
      <header className="mb-8">
        <h1 className="text-brand font-bold">Carbon Monitor</h1>
        <p className="text-gray-500 italic">Real-time Grid Updates via SignalR</p>
      </header>

      {/* Logic: Show loading if true, else show the logs list */}
      {loading ? (
        <div className="flex items-center justify-center p-12">
          <p className="animate-pulse text-accent">Fetching historical data...</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {logs.length === 0 ? (
            <p className="text-gray-400">No data collected yet. Waiting for worker...</p>
          ) : (
            logs.map((log, index) => (
              <div
                key={log.id || index}
                className="p-4 border border-border rounded-lg bg-social-bg flex justify-between items-center animate-in fade-in slide-in-from-top-4 duration-500"
              >
                <div>
                  <p className="text-sm text-gray-400">
                    {new Date(log.timestamp).toLocaleString()}
                  </p>
                  <p className="font-mono font-bold text-text-h">
                    {log.intensity} gCO2/kWh
                  </p>
                </div>
                <span className={`px-3 py-1 rounded-full text-xs font-bold ${log.rating === 'Green' ? 'bg-green-500/20 text-green-400' :
                    log.rating === 'Moderate' ? 'bg-yellow-500/20 text-yellow-400' :
                      'bg-red-500/20 text-red-400'
                  }`}>
                  {log.rating}
                </span>
              </div>
            ))
          )}
        </div>
      )}
    </div>
  )
}

export default App