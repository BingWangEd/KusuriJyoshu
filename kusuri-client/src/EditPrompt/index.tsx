import { useCallback, useState } from "react";
import DrawerNav from "../DrawerNav"
import { PrescriptionContextProvider } from "./usePrescriptionContext";
import ShowPrompts from "./ShowPrompts";
import AddIcon from '@mui/icons-material/Add';
import PromptEditor from "./PromptEditor";
import './editPrompt.css';

function EditPrompt() {
    const [adding, setAdding] = useState(false);
    const addPrompt = () => setAdding(true);
    const patientId = 1;
  
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
              <div className="editPromptContainer">
                <div className="promptHistory">
                  <div className="scrollContent">
                    <div className="buffer"></div>
                    <ShowPrompts />
                  </div>
                </div>
                  {adding && <PromptEditor label={"処方追加"} content={""} save={savePrompt} cancel={() => setAdding(false)} />}
                  {!adding && <button onClick={addPrompt} style={{display: 'inline-block'}}><AddIcon /></button>}
                </div>
            </PrescriptionContextProvider>
        </DrawerNav>
    )
}

export default EditPrompt;
