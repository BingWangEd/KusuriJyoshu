import { useCallback, useState } from "react";
import DrawerNav from "../DrawerNav"
import { PrescriptionContextProvider } from "./usePrescriptionContext";
import ShowPrompts from "./ShowPrompts";
import AddIcon from '@mui/icons-material/Add';
import PromptEditor from "./PromptEditor";

function EditPrompt() {
    const [adding, setAdding] = useState(false);
    const addPrompt = () => setAdding(true);
    const patientId = 2;
  
    const savePrompt = useCallback(async (content: string) => {
        try {
          const response = await fetch(`/api/PrescriptionPrompt/add/${patientId}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(content)
          });
    
          if (!response.ok) {
            throw new Error("Network response was not ok");
          }
    
          const result = await response.json();
          console.log(result);
        } catch (error) {
          console.error(error);
        }
    }, [setAdding]);


    return (
        <DrawerNav>
            <PrescriptionContextProvider patientId={2}>
                <ShowPrompts />
                {adding && <PromptEditor label={"処方追加"} content={""} save={savePrompt} cancel={() => setAdding(false)} />}
                {!adding && <button onClick={addPrompt}><AddIcon /></button>}
            </PrescriptionContextProvider>
        </DrawerNav>
    )
}

export default EditPrompt;
