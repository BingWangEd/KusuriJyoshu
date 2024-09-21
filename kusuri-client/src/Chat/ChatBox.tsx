import { TextField } from "@mui/material";
import { useCallback, useState } from "react";
import ArrowCircleUpIcon from '@mui/icons-material/ArrowCircleUp';

interface IChatBox {
	
}

const ChatBox = ({  }: IChatBox) => {
    const [message, setMessage] = useState("");
    const sendMessage = useCallback(() => {
        
    }, []);

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
