import { TextField } from "@mui/material";
import { useCallback, useState } from "react";
import ArrowCircleUpIcon from '@mui/icons-material/ArrowCircleUp';

interface IChatBox {

}

const ChatBox = ({  }: IChatBox) => {
    const patientId = 2;
    const [message, setMessage] = useState("");
    const sendMessage = useCallback(async () => {
        try {
            const response = await fetch(`/api/Chat/${patientId}`, {
              method: "POST",
              headers: { "Content-Type": "application/json" },
              body: JSON.stringify(message)
            });
      
            if (!response.ok) {
              throw new Error("Network response was not ok");
            }

            console.log(response);
        } catch (error) {
            console.error(error);
        }
    }, [message]);

	return (
        <div className="chatBox">
            <TextField
                id="outlined-multiline-static"
                label="メッセージを入力してください"
                multiline
                rows={1}
                value={message}
                onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                    setMessage(event.target.value);
                }}
                style={{ flexGrow: 1 }}
            />
            <button onClick={sendMessage} style={{ marginLeft: '8px'}}><ArrowCircleUpIcon /></button>
        </div>
	);
}

export default ChatBox;
