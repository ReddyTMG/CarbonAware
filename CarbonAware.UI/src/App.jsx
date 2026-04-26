import { useEffect, useState } from 'react'

function App() {
  const [logs, setLogs] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Replace with your actual .NET API port if different
    fetch('http://localhost:5258/carbon/history')
      .then(response => response.json())
      .then(data => {
        setLogs(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
        setLoading(false);
      });
  }, []);

  return (
    <div className="p-8">
      <header className="mb-8">
        <h1 className="text-brand font-bold">Carbon Monitor</h1>
        <p className="text-gray-500">Historical Grid Intensity Data</p>
      </header>

      {loading ? (
        <p>Loading grid data...</p>
      ) : (
        <div className="grid gap-4">
          {logs.map((log) => (
            <div key={log.id} className="p-4 border border-border rounded-lg bg-social-bg flex justify-between items-center">
              <div>
                <p className="text-sm text-gray-400">
                  {new Date(log.timestamp).toLocaleString()}
                </p>
                <p className="font-mono font-bold text-text-h">
                  {log.intensity} gCO2/kWh
                </p>
              </div>
              <span className={`px-3 py-1 rounded-full text-xs font-bold ${
                log.rating === 'Green' ? 'bg-green-500/20 text-green-400' : 
                log.rating === 'Moderate' ? 'bg-yellow-500/20 text-yellow-400' : 
                'bg-red-500/20 text-red-400'
              }`}>
                {log.rating}
              </span>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default App