import React, { createContext, useState, useEffect, FunctionComponent, ReactNode, useCallback } from 'react';

interface IPrompt{
    id: number;
    content: string;
    date: string;
    patientId: number;
}
interface IPrescriptionContext{
    prescriptions: IPrompt[];
    refetch: () => Promise<void>;
}

const PrescriptionContext = createContext<IPrescriptionContext>({
    prescriptions: [],
    refetch: async () => {
      console.log("Calling placeholder refetch.");
    },
});

interface IPrescriptionContextProvider {
  patientId: number;
  children: ReactNode
}

export const PrescriptionContextProvider: FunctionComponent<IPrescriptionContextProvider> = ({ patientId, children }) => {
  const [prompts, setPrompts] = useState<IPrompt[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchData = useCallback(async () => {
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
  }, [patientId]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return (
    <PrescriptionContext.Provider value={{ prescriptions: prompts, refetch: fetchData }}>
      {children}
    </PrescriptionContext.Provider>
  );
};

export const usePrescriptionContext = () => React.useContext(PrescriptionContext);
