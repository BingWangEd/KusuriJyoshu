import { Card, CardContent, Typography } from "@mui/material";
import { IChat } from "./ChatBox";

interface IChatHistory {
	history: IChat[]
}

const ChatHistory = ({ history }: IChatHistory) => {
	return (
		<>{history.map((h, index) => (
			<div key={index} className={`chatBubble ${h.byBot ? "botResponse" : "userPrompt"}`}>
				<Card sx={{ "maxWidth": "80%", width: "fit-content" }}>
					<CardContent>
						<Typography gutterBottom variant="body1" component="div">
							{h.createdAt}
						</Typography>
						<Typography variant="body2" sx={{ color: 'text.secondary' }}>
							{h.content}
						</Typography>
					</CardContent>
				</Card>
			</div>
		))
		}</>
	);
}

export default ChatHistory;
