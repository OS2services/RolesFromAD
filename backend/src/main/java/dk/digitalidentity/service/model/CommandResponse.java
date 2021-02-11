package dk.digitalidentity.service.model;

import java.util.List;

import dk.digitalidentity.controller.api.dto.UserDTO;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class CommandResponse {
	private boolean valid;
	private String message;
	private List<UserDTO> payload;
}
