package dk.digitalidentity.service;

import java.time.LocalDateTime;

import dk.digitalidentity.service.model.CommandResponse;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ResponseHolder {
	private CommandResponse response;
	private LocalDateTime tts;

	public ResponseHolder(CommandResponse response) {
		this.response = response;
		this.tts = LocalDateTime.now().plusMinutes(5L);
	}
}
