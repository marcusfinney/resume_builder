    public void SetupJsonObjects(ReactComponentModuleViewModel viewModel, Item moduleItem)
    {
      viewModel.ReactObjects = SetJsonString(moduleItem);
    }

    private JObject SetJsonString(Item modultItem)
    {
      var json = new JObject(new JProperty("components", BuildJsonObject(modultItem)));

      return json;
    }

    private JArray BuildJsonObject(Item item)
    {
      var json = new JArray();

      var vscObjects = _sitecoreItemService.GetFilteredChildren(item, i => i != null);
      if (vscObjects != null && vscObjects.Count > 0)
      {
        foreach (var obj in vscObjects)
        {
          json.Add(new JObject
          {
            new JProperty("name", obj.Name),
            new JProperty("type", obj.TemplateName),
            new JProperty("id", obj.ID.ToString()),
            new JProperty("properties", GetSitecoreItemProperties(obj))
          });
        }
      }

      return json;
    }

    private JObject GetSitecoreItemProperties(Item item)
    {
      var json = new JObject();

      foreach (Field field in item.Fields)
      {
        if (!field.Name.StartsWith("__"))
        {
          json.Add(new JProperty(field.Name, field.Value));
        }
      }

      json.Add(new JProperty("children", BuildJsonObject(item)));
      
      return json;
    }