import { usePrescriptionContext } from "./usePrescriptionContext";
import PromptCard from "./PromptCard";

function ShowPrompts() {
    const { prescriptions } = usePrescriptionContext();

    return (
        <>
            {
                prescriptions.length > 0 && prescriptions.map(
                    p => <PromptCard key={p.id} id={p.id} date={p.date} content={p.content} patientId={p.patientId}/>
                )
            }
        </>
    )
}

export default ShowPrompts;