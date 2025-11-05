import { useState } from 'react';
import { Button } from './components/ui/button';

function App() {
  const [count, setCount] = useState(0);

  return (
    <div className="flex flex-col items-center justify-center gap-5">
      <div>MERCIBUS APP: {count}</div>
      <Button onClick={() => setCount(count + 1)}>Increment</Button>
    </div>
  );
}

export default App;
