import { useCallback } from "react";
import DrawerNav from "../DrawerNav"
import { PrescriptionContextProvider } from "./usePrescriptionContext";

function EditPrompt() {
    const addPrompt = useCallback(async () => {
        try {
          const response = await fetch(`/api/PrescriptionPrompt/add?patientId=2`, {
            method: "POST",
            headers: {
              "Content-Type": "text/plain",
            },
            body: "hellooo"
          });
    
          if (!response.ok) {
            throw new Error("Network response was not ok");
          }
    
          const result = await response.json();
          console.log(result);
        } catch (error) {
          console.error(error);
        }
    }, []);

    return (
        <DrawerNav>
            <PrescriptionContextProvider patientId={2}>
                EditPrompt
                <button onClick={addPrompt}>add</button>
            </PrescriptionContextProvider>
        </DrawerNav>
    )
}

export default EditPrompt