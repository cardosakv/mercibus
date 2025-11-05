import { useState } from 'react';

function App() {
  const [count, setCount] = useState(0);

  return (
    <div className="flex flex-col items-center justify-center">
      <div>MERCIBUS APP: {count}</div>
      <button className="rounded-sm bg-blue-600" onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}

export default App;
