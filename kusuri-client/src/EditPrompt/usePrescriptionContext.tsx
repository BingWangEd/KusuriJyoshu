import React, { createContext, useState, useEffect, FunctionComponent, ReactNode } from 'react';

interface IPrompt{
    id: number;
    content: string;
    date: string;
    patientId: number;
}
const PrescriptionContext = createContext<IPrompt[]>([]);

interface IPrescriptionContextProvider {
  patientId: number;
  children: ReactNode
}

export const PrescriptionContextProvider: FunctionComponent<IPrescriptionContextProvider> = ({ patientId, children }) => {
  const [prompts, setPrompts] = useState<IPrompt[]>([]);
  const [loading, setLoading] = useState(true);

  // Fetch data from the backend
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(`/api/PrescriptionPrompt/history/${encodeURIComponent(patientId)}`);
        const result = await response.json();
        console.log("result: ", result);
        setPrompts(result);
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <PrescriptionContext.Provider value={prompts}>
      {children}
    </PrescriptionContext.Provider>
  );
};

// Export the context for use in other components
export const usePrescriptionContext = () => React.useContext(PrescriptionContext);
